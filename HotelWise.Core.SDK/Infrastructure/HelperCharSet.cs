using SmartCoreHub.Core.SDK.Common.Attributes;

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
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Infrastructure.Data.Configurations.Helper.Ported.HelperCharSet", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Infrastructure.Data.Configurations.Helper.Ported.HelperCharSet em SmartCoreHub.Core.SDK.")]
public static class HelperCharSet
{
    /// <summary>Conjunto de caracteres padrão (latin1) para entidades do SDK.</summary>
    public const string DefaultCharSet = "latin1";
}
