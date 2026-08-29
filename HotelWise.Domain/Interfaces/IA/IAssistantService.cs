using HotelWise.Domain.Dto.IA;

namespace HotelWise.Domain.Interfaces.IA
{
    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — implementação canônica no pacote Core.
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.AI.Abstractions.IAssistantService.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_AI")]
    public interface IAssistantService
    {
        Task<float[]?> GenerateEmbeddingAsync(string text);
        Task<AskAssistantResponse[]?> AskAssistant(AskAssistantRequest request);
        void SetUserId(long id);
    }
}
