﻿using HotelWise.Domain.Dto;
using HotelWise.Domain.Dto.AppConfig;
using HotelWise.Domain.Enuns.IA;
using HotelWise.Domain.Helpers;
using HotelWise.Domain.Interfaces;
using HotelWise.Domain.Interfaces.IA;
using Microsoft.Extensions.AI;
using OllamaSharp;
using OllamaSharp.Models.Chat;
using System.Text;

namespace HotelWise.Domain.AI.Adapter
{
    public class OllamaAdapter : IAIInferenceAdapter
    {
        private readonly OllamaApiClient _clientChat;
        private readonly OllamaApiClient _clientEmbedding;
         
        public OllamaAdapter(IApplicationIAConfig applicationConfig)
        {
            // Inicializa a configuração e o cliente Ollama
            var _config = (OllamaConfig)applicationConfig.GetChatServiceConfig(AIChatServiceType.OllamaAdapter)
                    ?? throw new InvalidOperationException("Ollama configuration is missing.");
            _clientChat = createClient(_config.Endpoint, _config.ModelId);
            _clientEmbedding = createClient(_config.Endpoint, _config.ModelId);
        }
        public OllamaApiClient GetClientChat()
        {
            return _clientChat;
        }
        public OllamaApiClient GetClientEmbedding()
        {
            return _clientEmbedding;
        }
        private static OllamaApiClient createClient(string url, string modelId)
        {
            var uri = new Uri(url);
            var clientInstance = new OllamaApiClient(uri);
            // Seleciona o modelo configurado - Chat
            clientInstance!.SelectedModel = modelId;
            return clientInstance;
        }

        public async Task<string> GenerateChatCompletionAsync(PromptMessageVO[] messages)
        {
            if (messages == null || !messages.Any())
                throw new ArgumentException("Messages cannot be null or empty.");

            try
            {
                // Cria uma instância de chat e envia as mensagens
                ChatRequest chatRequest = new ChatRequest();
                // Converte as mensagens para o formato de chat do Ollama
                var chatMessages = messages.Select(m => new Message
                {
                    Role = ConvertRole(m.Role),
                    Content = m.Content
                }).ToList();

                chatRequest.Messages = chatMessages;
                StringBuilder responseContent = new StringBuilder();

                await foreach (var stream in _clientChat.ChatAsync(chatRequest))
                {
                    var msgresult = stream!.Message.Content;
                    if (msgresult != null)
                    {
                        responseContent.Append(msgresult);
                    }
                }
                var result = responseContent.ToString();
                result = MarkdownHelper.ConvertToHtmlIfMarkdown(result);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error generating chat completion with Ollama: {ex.Message}", ex);
            }
        }

        public async Task<float[]> GenerateEmbeddingAsync(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Text cannot be null or empty.");

            try
            {
                // Chama o método GenerateEmbeddingAsync com os tipos explicitamente definidos
                var embedding = await _clientChat.GenerateEmbeddingAsync(
                    text,
                    options: null, // Pode passar opções se necessário
                    cancellationToken: CancellationToken.None
                );

                // Converte os embeddings em um array de floats
                // Converte o Vector para um array de float
                float[] floatArray = embedding.Vector.ToArray();

                return floatArray;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error generating embedding with Ollama: {ex.Message}", ex);
            }
        }
        private string ConvertRole(string role)
        {
            // Mapeia os papéis para o formato suportado pelo OllamaSharp
            return role.ToLower() switch
            {
                "system" => "system",
                "user" => "user",
                "assistant" => "assistant",
                _ => "user"
            };
        }
    }
}
