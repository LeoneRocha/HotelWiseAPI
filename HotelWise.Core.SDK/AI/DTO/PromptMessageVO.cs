using SchDto = SmartCoreHub.Core.SDK.Domain.AI.DTO;

namespace HotelWise.Core.SDK.AI.DTO;

/// <summary>
/// Fragmento de contexto vetorial embutido em prompts RAG.
/// </summary>
public class DataVectorVO : SchDto.DataVectorVO
{
}

/// <summary>
/// Mensagem de prompt para adapters de inferência — herda SCH.
/// </summary>
public class PromptMessageVO : SchDto.PromptMessageVO
{
}
