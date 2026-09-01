#if NET8_0_OR_GREATER
using System.Linq.Expressions;
using HotelWise.Core.SDK.Abstractions;
using Microsoft.EntityFrameworkCore;

using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.Infrastructure;

/// <summary>
/// Repositório genérico baseado em Entity Framework Core.
/// Fornece operações CRUD assíncronas padrão sobre um <see cref="DbSet{T}"/>,
/// permitindo que repositórios concretos herdem e especializem o comportamento
/// sem reimplementar o boilerplate de persistência.
/// </summary>
/// <typeparam name="T">Tipo da entidade de domínio mapeada.</typeparam>
/// <typeparam name="TContext">Tipo do <see cref="DbContext"/> utilizado.</typeparam>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Infrastructure.Repositories.Ported.GenericRepositoryBase", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Infrastructure.Repositories.Ported.GenericRepositoryBase em SmartCoreHub.Core.SDK.")]
public abstract class GenericRepositoryBase<T, TContext> : SmartCoreHub.Core.SDK.Infrastructure.Repositories.Ported.GenericRepositoryBase<T, TContext>, IGenericRepository<T>
    where T : class
    where TContext : DbContext
{
    /// <summary>
    /// Inicializa uma nova instância de <see cref="GenericRepositoryBase{T, TContext}"/>.
    /// </summary>
    /// <param name="context">Instância do DbContext.</param>
    /// <param name="options">Opções de configuração do DbContext.</param>
    protected GenericRepositoryBase(TContext context, DbContextOptions<TContext> options)
        : base(context, options)
    {
    }
}
#endif
