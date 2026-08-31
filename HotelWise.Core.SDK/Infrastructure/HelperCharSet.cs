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
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Infrastructure. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Infrastructure.Data.Configurations.Helper.Ported.HelperCharSet. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public static class HelperCharSet
{
    /// <summary>Conjunto de caracteres padrão (latin1) para entidades do SDK.</summary>
    public const string DefaultCharSet = "latin1";
}
