#if NET8_0_OR_GREATER
using System.Text.Json.Nodes;
using GroqApiLibrary;
using HotelWise.Core.SDK.AI.Abstractions;
using HotelWise.Core.SDK.AI.DTO;
using HotelWise.Core.SDK.AI.Enums;

namespace HotelWise.Core.SDK.AI.Adapters;

/// <summary>
/// Adapter de inferência via Groq API.
/// </summary>
public class GroqApiAdapter : IAIInferenceAdapter
{
    private readonly GroqApiClient _groqApiClient;
    private readonly string _model;

    public GroqApiAdapter(IApplicationIAConfig applicationConfig)
    {
        _groqApiClient = new GroqApiClient(applicationConfig.GroqApiConfig.ApiKey);
        _model = applicationConfig.GroqApiConfig.ModelId;
    }

    public async Task<string> GenerateChatCompletionAsync(PromptMessageVO[] messages)
    {
        var request = new JsonObject
        {
            ["model"] = _model,
            ["messages"] = new JsonArray(messages.Select(m => new JsonObject
            {
                ["role"] = GetRole(m.RoleType),
                ["content"] = m.Content
            }).ToArray())
        };

        var result = await _groqApiClient.CreateChatCompletionAsync(request);
        var resultOut = result?["choices"]?[0]?["message"]?["content"]?.ToString();
        return resultOut ?? string.Empty;
    }

    private static string GetRole(RoleAiPromptsType roleType) =>
        roleType switch
        {
            RoleAiPromptsType.System => "system",
            RoleAiPromptsType.Agent => "system",
            RoleAiPromptsType.User => "user",
            RoleAiPromptsType.Assistant => "assistant",
            _ => "user"
        };

    public async Task<string> GenerateChatCompletionByAgentAsync(PromptMessageVO[] messages) =>
        await GenerateChatCompletionAsync(messages);

    public Task<float[]> GenerateEmbeddingAsync(string text) =>
        throw new NotImplementedException();

    public async Task<string> GenerateChatCompletionByAgentSimpleRagAsync(PromptMessageVO[] messages) =>
        await GenerateChatCompletionAsync(messages);
}
#endif
