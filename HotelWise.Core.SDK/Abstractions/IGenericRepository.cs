using SmartCoreHub.Core.SDK.Domain.Entities.Common;

using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.Abstractions;

/// <summary>
/// Contrato genérico de repositório canônico (PR-D6) — <typeparamref name="T"/> : <see cref="LongEntityBase"/>.
/// </summary>
/// <typeparam name="T">Tipo da entidade de domínio gerenciada pelo repositório.</typeparam>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Infrastructure.Repositories.Generic.IGenericRepository`1", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Infrastructure.Repositories.Generic.IGenericRepository`1 em SmartCoreHub.Core.SDK.")]
public interface IGenericRepository<T> : SmartCoreHub.Core.SDK.Infrastructure.Repositories.Generic.IGenericRepository<T>
    where T : LongEntityBase
{
}
