using OllamaSharp;
namespace HotelWise.ConsolePOC
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                // Configuração do cliente Ollama
                var uri = new Uri("http://localhost:11434"); // Atualize se necessário
                var ollama = new OllamaApiClient(uri);

                // Selecione o modelo que será usado
                ollama.SelectedModel = "llama3.2";

                Console.WriteLine("Ollama Client Initialized.");
                Console.WriteLine($"Selected Model: {ollama.SelectedModel}");

                // Listando os modelos locais disponíveis
                Console.WriteLine("\nAvailable Local Models:");
                var models = await ollama.ListLocalModelsAsync();
                foreach (var model in models)
                {
                    Console.WriteLine($"- {model}");
                }

                // Exemplo: Gerar uma conclusão
                Console.WriteLine("\nGenerating a response...");
                await foreach (var stream in ollama.GenerateAsync("ME fala sobre recife?"))
                {
                    Console.Write(stream!.Response);
                }

                // Exemplo: Construir um chat interativo
                Console.WriteLine("\n\nStarting interactive chat:");
                var chat = new Chat(ollama);

                while (true)
                {
                    Console.Write("\nYou: ");
                    var message = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(message))
                    {
                        Console.WriteLine("Exiting chat...");
                        break;
                    }

                    Console.Write("Assistant: ");
                    await foreach (var answerToken in chat.SendAsync(message))
                    {
                        Console.Write(answerToken);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }


}
