using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.MistralAI;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace HotelWise.ConsolePOC;

/// <summary>
/// POC para isolar chamadas Mistral / LM Studio (OpenAI-compatible).
/// </summary>
static class Program
{
    private const string DefaultMistralModel = "mistral-medium-latest";
    private const string MistralChatUrl = "https://api.mistral.ai/v1/chat/completions";

    static async Task<int> Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("=== HotelWise.ConsolePOC — AI diagnostic ===\n");

        var config = BuildConfiguration();
        var mode = ResolveMode(args, config);
        var prompt = args.FirstOrDefault(a => !a.StartsWith("--", StringComparison.Ordinal))
                     ?? "Responda só com a palavra OK.";

        return mode switch
        {
            "lmstudio" or "openai" or "local" => await RunLmStudioAsync(config, args, prompt),
            _ => await RunMistralAsync(config, args, prompt),
        };
    }

    private static string ResolveMode(string[] args, IConfiguration config)
    {
        var fromArg = args.FirstOrDefault(a => a.StartsWith("--mode=", StringComparison.OrdinalIgnoreCase));
        if (!string.IsNullOrWhiteSpace(fromArg))
        {
            return fromArg["--mode=".Length..].Trim().ToLowerInvariant();
        }

        var chatApi = config["ApplicationIAConfig:Rag:AIChatServiceApi"] ?? string.Empty;
        return chatApi.Equals("OpenAI", StringComparison.OrdinalIgnoreCase) ? "lmstudio" : "mistral";
    }

    private static async Task<int> RunLmStudioAsync(IConfiguration config, string[] args, string prompt)
    {
        var endpoint = config["ApplicationIAConfig:AIServices:OpenAI:Endpoint"]
                       ?? "http://192.168.15.21:1234/v1";
        var modelId = config["ApplicationIAConfig:AIServices:OpenAI:ModelId"]
                      ?? "shisa-v2-mistral-nemo-12b-abliterated-i1";
        var apiKey = ResolveOpenAiKey(config, args);

        Console.WriteLine("Mode  : LM Studio / OpenAI-compatible");
        Console.WriteLine($"URL   : {endpoint}");
        Console.WriteLine($"Model : {modelId}");
        Console.WriteLine($"ApiKey: {MaskKey(apiKey)}");
        Console.WriteLine($"Prompt: \"{prompt}\"\n");

        Console.WriteLine("--- 1) HTTP direto (/v1/chat/completions) ---");
        var chatUrl = endpoint.TrimEnd('/') + "/chat/completions";
        var httpOk = await RunRawOpenAiCompatibleAsync(chatUrl, apiKey, modelId, prompt);

        Console.WriteLine("\n--- 2) Semantic Kernel (AddOpenAIChatCompletion) ---");
        var skOk = await RunSemanticKernelOpenAiAsync(endpoint, apiKey, modelId, prompt);

        Console.WriteLine("\n=== Resumo ===");
        Console.WriteLine($"HTTP direto     : {(httpOk ? "OK" : "FALHOU")}");
        Console.WriteLine($"Semantic Kernel : {(skOk ? "OK" : "FALHOU")}");
        return httpOk && skOk ? 0 : 2;
    }

    private static async Task<int> RunMistralAsync(IConfiguration config, string[] args, string prompt)
    {
        var apiKey = ResolveMistralKey(config, args);
        var modelId = config["ApplicationIAConfig:AIServices:MistralApi:ModelId"] ?? DefaultMistralModel;

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            Console.WriteLine("API key Mistral não encontrada. Use --mode=lmstudio para LM Studio local.");
            return 1;
        }

        Console.WriteLine("Mode  : Mistral cloud");
        Console.WriteLine($"Model : {modelId}");
        Console.WriteLine($"ApiKey: {MaskKey(apiKey)}");
        Console.WriteLine($"Prompt: \"{prompt}\"\n");

        Console.WriteLine("--- 1) HTTP direto (api.mistral.ai) ---");
        var httpOk = await RunRawOpenAiCompatibleAsync(MistralChatUrl, apiKey, modelId, prompt);

        Console.WriteLine("\n--- 2) Semantic Kernel (AddMistralChatCompletion) ---");
        var skOk = await RunSemanticKernelMistralAsync(apiKey, modelId, prompt);

        Console.WriteLine("\n=== Resumo ===");
        Console.WriteLine($"HTTP direto     : {(httpOk ? "OK" : "FALHOU")}");
        Console.WriteLine($"Semantic Kernel : {(skOk ? "OK" : "FALHOU")}");
        if (!httpOk || !skOk)
        {
            Console.WriteLine("\nSe 429: conta/quota Mistral. Alternativa: --mode=lmstudio");
            return 2;
        }

        return 0;
    }

    private static IConfiguration BuildConfiguration()
    {
        var apiDevSettings = Path.GetFullPath(Path.Combine(
            AppContext.BaseDirectory, "..", "..", "..", "..", "HotelWise.API", "appsettings.Development.json"));

        var builder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables();

        if (File.Exists(apiDevSettings))
        {
            builder.AddJsonFile(apiDevSettings, optional: false, reloadOnChange: false);
            Console.WriteLine($"Config: {apiDevSettings}");
        }

        return builder.Build();
    }

    private static string ResolveMistralKey(IConfiguration config, string[] args)
    {
        var fromArg = args.FirstOrDefault(a => a.StartsWith("--api-key=", StringComparison.OrdinalIgnoreCase));
        if (!string.IsNullOrWhiteSpace(fromArg))
        {
            return fromArg["--api-key=".Length..].Trim();
        }

        return config["ApplicationIAConfig:AIServices:MistralApi:ApiKey"]
               ?? Environment.GetEnvironmentVariable("MISTRAL_API_KEY")
               ?? string.Empty;
    }

    private static string ResolveOpenAiKey(IConfiguration config, string[] args)
    {
        var fromArg = args.FirstOrDefault(a => a.StartsWith("--api-key=", StringComparison.OrdinalIgnoreCase));
        if (!string.IsNullOrWhiteSpace(fromArg))
        {
            return fromArg["--api-key=".Length..].Trim();
        }

        var key = config["ApplicationIAConfig:AIServices:OpenAI:ApiKey"]
                  ?? Environment.GetEnvironmentVariable("OPENAI_API_KEY")
                  ?? "lm-studio";
        return string.IsNullOrWhiteSpace(key) ? "lm-studio" : key;
    }

    private static async Task<bool> RunRawOpenAiCompatibleAsync(string chatUrl, string apiKey, string modelId, string prompt)
    {
        using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(120) };
        using var request = new HttpRequestMessage(HttpMethod.Post, chatUrl);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var payload = new
        {
            model = modelId,
            messages = new[] { new { role = "user", content = prompt } },
            max_tokens = 16,
            temperature = 0.0
        };

        request.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

        var started = DateTimeOffset.Now;
        using var response = await http.SendAsync(request);
        var elapsed = DateTimeOffset.Now - started;
        var body = await response.Content.ReadAsStringAsync();

        Console.WriteLine($"Status : {(int)response.StatusCode} {response.StatusCode} ({elapsed.TotalMilliseconds:0} ms)");
        PrintRateLimitHeaders(response);

        if (response.IsSuccessStatusCode)
        {
            TryPrintChatContent(body);
            return true;
        }

        Console.WriteLine("Body   :");
        Console.WriteLine(Truncate(body, 2000));
        return false;
    }

    private static async Task<bool> RunSemanticKernelOpenAiAsync(string endpoint, string apiKey, string modelId, string prompt)
    {
        try
        {
#pragma warning disable SKEXP0010
            var kernel = Kernel.CreateBuilder()
                .AddOpenAIChatCompletion(modelId: modelId, endpoint: new Uri(endpoint), apiKey: apiKey)
                .Build();
#pragma warning restore SKEXP0010

            var chat = kernel.GetRequiredService<IChatCompletionService>();
            var history = new ChatHistory();
            history.AddUserMessage(prompt);

            var started = DateTimeOffset.Now;
            var result = await chat.GetChatMessageContentAsync(history);
            var elapsed = DateTimeOffset.Now - started;

            Console.WriteLine($"Status : OK ({elapsed.TotalMilliseconds:0} ms)");
            Console.WriteLine($"Reply  : {Truncate(result.Content ?? string.Empty, 500)}");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Status : FALHOU");
            Console.WriteLine($"Error  : {ex.GetType().Name}: {ex.Message}");
            return false;
        }
    }

    private static async Task<bool> RunSemanticKernelMistralAsync(string apiKey, string modelId, string prompt)
    {
        try
        {
            var kernel = Kernel.CreateBuilder()
                .AddMistralChatCompletion(modelId: modelId, apiKey: apiKey)
                .Build();

            var chat = kernel.GetRequiredService<IChatCompletionService>();
            var history = new ChatHistory();
            history.AddUserMessage(prompt);

            var started = DateTimeOffset.Now;
            var result = await chat.GetChatMessageContentAsync(history);
            var elapsed = DateTimeOffset.Now - started;

            Console.WriteLine($"Status : OK ({elapsed.TotalMilliseconds:0} ms)");
            Console.WriteLine($"Reply  : {Truncate(result.Content ?? string.Empty, 500)}");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Status : FALHOU");
            Console.WriteLine($"Error  : {ex.GetType().Name}: {ex.Message}");
            if (ex.InnerException is not null)
            {
                Console.WriteLine($"Inner  : {ex.InnerException.Message}");
            }
            return false;
        }
    }

    private static void PrintRateLimitHeaders(HttpResponseMessage response)
    {
        var interesting = response.Headers
            .Concat(response.Content.Headers)
            .Where(h =>
                h.Key.Contains("rate", StringComparison.OrdinalIgnoreCase)
                || h.Key.Contains("retry", StringComparison.OrdinalIgnoreCase)
                || h.Key.Contains("limit", StringComparison.OrdinalIgnoreCase)
                || h.Key.Equals("x-request-id", StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (interesting.Count == 0)
        {
            Console.WriteLine("Headers: (nenhum header de rate-limit/retry visível)");
            return;
        }

        Console.WriteLine("Headers:");
        foreach (var header in interesting)
        {
            Console.WriteLine($"  {header.Key}: {string.Join(", ", header.Value)}");
        }
    }

    private static void TryPrintChatContent(string body)
    {
        try
        {
            using var doc = JsonDocument.Parse(body);
            var content = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();
            Console.WriteLine($"Reply  : {Truncate(content ?? string.Empty, 500)}");
        }
        catch
        {
            Console.WriteLine($"Body   : {Truncate(body, 500)}");
        }
    }

    private static string MaskKey(string key)
    {
        if (key.Length <= 8)
        {
            return "****";
        }

        return $"{key[..4]}...{key[^4..]} (len={key.Length})";
    }

    private static string Truncate(string value, int max)
        => value.Length <= max ? value : value[..max] + "...";
}
