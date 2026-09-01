using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.AI.Abstractions;

/// <summary>
/// Contrato de dado vetorial genérico usado no vector store e no pipeline RAG.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IDataVector", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IDataVector em SmartCoreHub.Core.SDK.")]
public interface IDataVector : SmartCoreHub.Core.SDK.Domain.AI.Abstractions.IDataVector
{
}
