using SchDto = SmartCoreHub.Core.SDK.Domain.AI.DTO;

namespace HotelWise.Core.SDK.AI.DTO;

/// <summary>
/// Fragmento de contexto vetorial embutido em prompts RAG.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.DTO.DataVectorVO. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class DataVectorVO : SchDto.DataVectorVO
{
}

/// <summary>
/// Mensagem de prompt para adapters de inferência — herda SCH.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.DTO.PromptMessageVO. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class PromptMessageVO : SchDto.PromptMessageVO
{
}
