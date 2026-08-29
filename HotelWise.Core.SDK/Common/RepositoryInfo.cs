namespace HotelWise.Core.SDK.Common;

/// <summary>
/// Par interface/implementação usado no registro de repositórios em injeção de dependência.
/// Associa o tipo do contrato ao tipo concreto a ser resolvido pelo container.
/// </summary>
public class RepositoryInfo
{
    /// <summary>
    /// Tipo da interface (contrato) do repositório; pode ser <c>null</c> se ainda não definido.
    /// </summary>
    public Type? InterfaceType { get; set; }

    /// <summary>
    /// Tipo da implementação concreta do repositório; pode ser <c>null</c> se ainda não definido.
    /// </summary>
    public Type? ImplementationType { get; set; }
}
