
namespace HotelWise.Core.SDK.Infrastructure;

/// <summary>
/// Constantes de charset utilizadas em mapeamentos Entity Framework.
/// Centraliza o conjunto de caracteres padrão do SDK; a aplicação
/// provider-specific (ex.: Pomelo <c>HasCharSet</c>) permanece no host.
/// </summary>
/// <remarks>
/// Exemplo de uso em configuração de entidade:
/// <code>
/// builder.Property(e => e.Nome)
///     .HasCharSet(HelperCharSet.DefaultCharSet);
/// </code>
/// </remarks>
public static class HelperCharSet
{
    /// <summary>Conjunto de caracteres padrão (latin1) para entidades do SDK.</summary>
    public const string DefaultCharSet = "latin1";
}
