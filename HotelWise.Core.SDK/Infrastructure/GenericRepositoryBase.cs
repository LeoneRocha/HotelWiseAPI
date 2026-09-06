#if NET8_0_OR_GREATER
using HotelWise.Core.SDK.Abstractions;
using Microsoft.EntityFrameworkCore;
using SmartCoreHub.Core.SDK.Domain.Entities.Common;
using SmartCoreHub.Core.SDK.Domain.Interfaces.Common;
using SmartCoreHub.Core.SDK.EntityFrameworkCore.Repositories;

using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.Infrastructure;

/// <summary>
/// Repositório genérico canônico (PR-D6) baseado em Entity Framework Core.
/// </summary>
/// <typeparam name="T">Tipo da entidade de domínio mapeada (<see cref="LongEntityBase"/>).</typeparam>
/// <typeparam name="TContext">Tipo do <see cref="DbContext"/> utilizado.</typeparam>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.EntityFrameworkCore.Repositories.GenericRepository`2", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.EntityFrameworkCore.Repositories.GenericRepository`2 em SmartCoreHub.Core.SDK.")]
public abstract class GenericRepositoryBase<T, TContext> : GenericRepository<T, TContext>, IGenericRepository<T>
    where T : LongEntityBase
    where TContext : DbContext
{
    /// <summary>
    /// Inicializa uma nova instância de <see cref="GenericRepositoryBase{T, TContext}"/>.
    /// </summary>
    protected GenericRepositoryBase(TContext context, DbContextOptions<TContext> options)
        : base(context, NullAppLogger.Instance, options)
    {
    }
}
#endif
