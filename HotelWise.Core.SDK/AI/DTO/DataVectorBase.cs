#if NET8_0_OR_GREATER
using HotelWise.Core.SDK.AI.Abstractions;

namespace HotelWise.Core.SDK.AI.DTO;

/// <summary>
/// Base de dado vetorial com atributos de vector store.
/// </summary>
public abstract class DataVectorBase : SmartCoreHub.Core.SDK.Domain.AI.DTO.DataVectorBase, IDataVector
{
}
#endif
