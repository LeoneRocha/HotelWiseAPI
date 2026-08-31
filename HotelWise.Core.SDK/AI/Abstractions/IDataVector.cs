namespace HotelWise.Core.SDK.AI.Abstractions;

/// <summary>
/// Contrato de dado vetorial genérico usado no vector store e no pipeline RAG.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IDataVector. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public interface IDataVector : SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IDataVector
{
}
