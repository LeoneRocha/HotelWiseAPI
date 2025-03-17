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
        private OllamaApiClient _client;
        private readonly string _modelId;
        private readonly OllamaConfig _config;

        public OllamaAdapter(IApplicationIAConfig applicationConfig)
        {
            // Inicializa a configuração e o cliente Ollama
            _config = (OllamaConfig)applicationConfig.GetChatServiceConfig(AIChatServiceType.OllamaAdapter)
                ?? throw new InvalidOperationException("Ollama configuration is missing.");
            fetchClient(_config.Endpoint);
            _modelId = _config.ModelId;

            // Seleciona o modelo configurado
            _client!.SelectedModel = _modelId;
        }

        private void fetchClient(string url)
        {
            var uri = new Uri(url);
            _client = new OllamaApiClient(uri);
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

                await foreach (var stream in _client.ChatAsync(chatRequest))
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
                fetchClient(_config.EndpointEmbeddings);
                _client.SelectedModel = _config.ModelIdEmbeddings;

                // Chama o método GenerateEmbeddingAsync com os tipos explicitamente definidos
                var embedding = await _client.GenerateEmbeddingAsync(
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
