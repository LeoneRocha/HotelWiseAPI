#if NET8_0_OR_GREATER
using HotelWise.Core.SDK.AI.Abstractions;

using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.AI.DTO;

/// <summary>
/// Base de dado vetorial com atributos de vector store.
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Domain.AI.DTO.DataVectorBase", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Domain.AI.DTO.DataVectorBase em SmartCoreHub.Core.SDK.")]
public abstract class DataVectorBase : SmartCoreHub.Core.SDK.Domain.AI.DTO.DataVectorBase, IDataVector
{
}
#endif
