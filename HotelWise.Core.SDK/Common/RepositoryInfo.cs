namespace HotelWise.Core.SDK.Common;

/// <summary>
/// Par interface/implementação usado no registro de repositórios em injeção de dependência.
/// Associa o tipo do contrato ao tipo concreto a ser resolvido pelo container.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Common. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Common.RepositoryInfo. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
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
