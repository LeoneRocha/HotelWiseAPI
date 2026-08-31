#if NET8_0_OR_GREATER
using HotelWise.Core.SDK.Common.Constants;
using Microsoft.Extensions.Configuration;

namespace HotelWise.Core.SDK.Helpers;

/// <summary>
/// Leitura tipada de seções e connection strings a partir de <see cref="IConfiguration"/>.
/// Centraliza nomes de seções conhecidos (Rag, TokenConfigurations, AzureAd, MySQL)
/// e valida presença da configuração antes do acesso.
/// </summary>
/// <example>
/// <code>
/// var tokenSection = ConfigurationAppSettingsHelper.GetTokenConfigurations(configuration);
/// var mysql = ConfigurationAppSettingsHelper.GetConnectionStringMySQL(configuration);
/// </code>
/// </example>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Domain. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Domain.Helpers.ConfigurationAppSettingsHelper. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public static class ConfigurationAppSettingsHelper
{
    /// <summary>
    /// Obtém uma seção nomeada da configuração da aplicação.
    /// </summary>
    /// <param name="configuration">Raiz de configuração; não pode ser nula.</param>
    /// <param name="sectionName">Nome da seção (ex.: "Rag").</param>
    /// <returns>A seção <see cref="IConfiguration"/> correspondente.</returns>
    /// <exception cref="ArgumentNullException">Quando <paramref name="configuration"/> é nula.</exception>
    public static IConfiguration GetSectionApp(IConfiguration? configuration, string sectionName)
    {
        if (configuration == null)
        {
            throw new ArgumentNullException(nameof(configuration), AppConfigConstants.ConfigurationConfigurationNotBeNull);
        }
        return configuration.GetSection(sectionName);
    }

    /// <summary>
    /// Obtém uma connection string pelo nome.
    /// </summary>
    /// <param name="configuration">Raiz de configuração; não pode ser nula.</param>
    /// <param name="connectionName">Nome da connection string.</param>
    /// <returns>Valor da connection string ou string vazia se ausente.</returns>
    /// <exception cref="ArgumentNullException">Quando <paramref name="configuration"/> é nula.</exception>
    public static string GetConnectionStringApp(IConfiguration? configuration, string connectionName)
    {
        if (configuration == null)
        {
            throw new ArgumentNullException(nameof(configuration), AppConfigConstants.ConfigurationConfigurationNotBeNull);
        }
        return configuration.GetConnectionString(connectionName) ?? string.Empty;
    }

    /// <summary>
    /// Obtém o valor string de uma chave de configuração (índice simples).
    /// </summary>
    /// <param name="configuration">Raiz de configuração; não pode ser nula.</param>
    /// <param name="configurationName">Nome da chave (ex.: "APP_ENVIRONMENT").</param>
    /// <returns>Valor da chave ou string vazia se ausente.</returns>
    /// <exception cref="ArgumentNullException">Quando <paramref name="configuration"/> é nula.</exception>
    public static string GetValueStringConfiguration(IConfiguration? configuration, string configurationName)
    {
        if (configuration == null)
        {
            throw new ArgumentNullException(nameof(configuration), AppConfigConstants.ConfigurationConfigurationNotBeNull);
        }
        string appsettingsValue = configuration[configurationName] ?? string.Empty;
        return appsettingsValue;
    }

    /// <summary>
    /// Obtém a connection string MySQL padrão (<c>DBConnectionMySQL</c>).
    /// </summary>
    /// <param name="configuration">Raiz de configuração; não pode ser nula.</param>
    /// <returns>Connection string MySQL ou string vazia.</returns>
    public static string GetConnectionStringMySQL(IConfiguration? configuration)
    {
        return GetConnectionStringApp(configuration, "DBConnectionMySQL");
    }

    /// <summary>
    /// Obtém a seção de configuração RAG.
    /// </summary>
    /// <param name="configuration">Raiz de configuração.</param>
    /// <returns>Seção <c>Rag</c>.</returns>
    public static IConfiguration GetRagConfig(IConfiguration configuration)
    {
        return GetSectionApp(configuration, "Rag");
    }

    /// <summary>
    /// Obtém a seção de configuração de tokens JWT.
    /// </summary>
    /// <param name="configuration">Raiz de configuração; não pode ser nula.</param>
    /// <returns>Seção <c>TokenConfigurations</c>.</returns>
    public static IConfiguration GetTokenConfigurations(IConfiguration? configuration)
    {
        return GetSectionApp(configuration, "TokenConfigurations");
    }

    /// <summary>
    /// Obtém a seção de configuração Azure AD.
    /// </summary>
    /// <param name="configuration">Raiz de configuração.</param>
    /// <returns>Seção <c>AzureAd</c>.</returns>
    public static IConfiguration GetAzureAdConfig(IConfiguration configuration)
    {
        return GetSectionApp(configuration, "AzureAd");
    }
}
#endif
