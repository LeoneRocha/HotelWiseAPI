#if NET8_0_OR_GREATER
using System.Linq.Expressions;
using HotelWise.Core.SDK.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace HotelWise.Core.SDK.Infrastructure;

/// <summary>
/// Repositório genérico baseado em Entity Framework Core.
/// Fornece operações CRUD assíncronas padrão sobre um <see cref="DbSet{T}"/>,
/// permitindo que repositórios concretos herdem e especializem o comportamento
/// sem reimplementar o boilerplate de persistência.
/// </summary>
/// <typeparam name="T">Tipo da entidade de domínio mapeada.</typeparam>
/// <typeparam name="TContext">Tipo do <see cref="DbContext"/> utilizado.</typeparam>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Infrastructure. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Infrastructure.Repositories.Ported.GenericRepositoryBase. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public abstract class GenericRepositoryBase<T, TContext> : SmartCoreHub.Core.SDK.Infrastructure.Repositories.Ported.GenericRepositoryBase<T, TContext>, IGenericRepository<T>
    where T : class
    where TContext : DbContext
{
    protected GenericRepositoryBase(TContext context, DbContextOptions<TContext> options)
        : base(context, options)
    {
    }
}

#endif
