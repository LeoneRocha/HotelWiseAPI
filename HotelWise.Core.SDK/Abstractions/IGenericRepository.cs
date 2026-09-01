using System.Linq.Expressions;

using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.Abstractions;

/// <summary>
/// Contrato genérico de repositório para persistência de entidades de domínio.
/// Define operações CRUD, consulta por predicado, contagem, paginação e verificação de existência
/// sobre o tipo <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">Tipo da entidade de domínio gerenciada pelo repositório.</typeparam>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Domain.Abstractions.IGenericRepository", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Domain.Abstractions.IGenericRepository em SmartCoreHub.Core.SDK.")]
public interface IGenericRepository<T> : SmartCoreHub.Core.SDK.Domain.Abstractions.IGenericRepository<T>
    where T : class
{
}
