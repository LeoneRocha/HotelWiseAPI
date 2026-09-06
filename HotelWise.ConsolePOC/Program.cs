using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.MistralAI;

namespace HotelWise.ConsolePOC;

/// <summary>
/// POC para isolar chamadas à Mistral (HTTP direto + Semantic Kernel),
/// útil para diagnosticar 429 / rate limit fora do HotelWise.API.
/// </summary>
static class Program
{
    private const string DefaultModel = "mistral-medium-latest";
    private const string ChatUrl = "https://api.mistral.ai/v1/chat/completions";

    static async Task<int> Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("=== HotelWise.ConsolePOC — Mistral diagnostic ===\n");

        var config = BuildConfiguration();
        var apiKey = ResolveApiKey(config, args);
        var modelId = config["ApplicationIAConfig:AIServices:MistralApi:ModelId"] ?? DefaultModel;

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            Console.WriteLine("API key não encontrada.");
            Console.WriteLine("Informe via:");
            Console.WriteLine("  - appsettings (ApplicationIAConfig:AIServices:MistralApi:ApiKey)");
            Console.WriteLine("  - env MISTRAL_API_KEY");
            Console.WriteLine("  - arg --api-key=<key>");
            return 1;
        }

        Console.WriteLine($"Model : {modelId}");
        Console.WriteLine($"ApiKey: {MaskKey(apiKey)}");
        Console.WriteLine($"Prompt: \"Responda só com a palavra OK.\"\n");

        var prompt = args.FirstOrDefault(a => !a.StartsWith("--", StringComparison.Ordinal))
                     ?? "Responda só com a palavra OK.";

        Console.WriteLine("--- 1) HTTP direto (api.mistral.ai) ---");
        var httpOk = await RunRawHttpAsync(apiKey, modelId, prompt);

        Console.WriteLine("\n--- 2) Semantic Kernel (AddMistralChatCompletion) ---");
        var skOk = await RunSemanticKernelAsync(apiKey, modelId, prompt);

        Console.WriteLine("\n=== Resumo ===");
        Console.WriteLine($"HTTP direto     : {(httpOk ? "OK" : "FALHOU")}");
        Console.WriteLine($"Semantic Kernel : {(skOk ? "OK" : "FALHOU")}");

        if (!httpOk || !skOk)
        {
            Console.WriteLine("\nSe ambos falham com 429, o problema é conta/quota/tier Mistral, não a lógica do HotelWise.");
            Console.WriteLine("Cheque: https://admin.mistral.ai/plateforme/limits");
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
        else
        {
            Console.WriteLine($"AVISO: não achei appsettings da API em:\n  {apiDevSettings}");
            Console.WriteLine("Usando só env / appsettings locais do POC.\n");
        }

        return builder.Build();
    }

    private static string ResolveApiKey(IConfiguration config, string[] args)
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

    private static async Task<bool> RunRawHttpAsync(string apiKey, string modelId, string prompt)
    {
        using var http = new HttpClient { Timeout = TimeSpan.FromSeconds(60) };
        using var request = new HttpRequestMessage(HttpMethod.Post, ChatUrl);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var payload = new
        {
            model = modelId,
            messages = new[]
            {
                new { role = "user", content = prompt }
            },
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

    private static async Task<bool> RunSemanticKernelAsync(string apiKey, string modelId, string prompt)
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
            Console.WriteLine($"Status : FALHOU");
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
