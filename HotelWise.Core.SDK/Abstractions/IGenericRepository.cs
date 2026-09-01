using System.Linq.Expressions;

namespace HotelWise.Core.SDK.Abstractions;

/// <summary>
/// Contrato genérico de repositório para persistência de entidades de domínio.
/// Define operações CRUD, consulta por predicado, contagem, paginação e verificação de existência
/// sobre o tipo <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">Tipo da entidade de domínio gerenciada pelo repositório.</typeparam>
public interface IGenericRepository<T> : SmartCoreHub.Core.SDK.Domain.Abstractions.IGenericRepository<T>
    where T : class
{
}
