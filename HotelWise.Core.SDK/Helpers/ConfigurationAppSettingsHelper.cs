#if NET8_0_OR_GREATER
using HotelWise.Core.SDK.Common.Constants;
using Microsoft.Extensions.Configuration;

using SmartCoreHub.Core.SDK.Common.Attributes;

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
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Domain.Helpers.ConfigurationAppSettingsHelper", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Domain.Helpers.ConfigurationAppSettingsHelper em SmartCoreHub.Core.SDK.")]
public static class ConfigurationAppSettingsHelper
{
    /// <summary>Obtém uma seção de configuração validando sua existência.</summary>
    /// <param name="configuration">Gerenciador de configuração.</param>
    /// <param name="sectionName">Nome da seção.</param>
    /// <returns>Seção de configuração correspondente.</returns>
    public static IConfiguration GetSectionApp(IConfiguration? configuration, string sectionName) =>
        SmartCoreHub.Core.SDK.Domain.Helpers.ConfigurationAppSettingsHelper.GetSectionApp(configuration, sectionName);

    /// <summary>Obtém uma connection string com fallback de validação.</summary>
    /// <param name="configuration">Gerenciador de configuração.</param>
    /// <param name="connectionName">Nome da connection string.</param>
    /// <returns>String de conexão encontrada.</returns>
    public static string GetConnectionStringApp(IConfiguration? configuration, string connectionName) =>
        SmartCoreHub.Core.SDK.Domain.Helpers.ConfigurationAppSettingsHelper.GetConnectionStringApp(configuration, connectionName);

    /// <summary>Obtém o valor em string de uma chave de configuração.</summary>
    /// <param name="configuration">Gerenciador de configuração.</param>
    /// <param name="configurationName">Nome da chave.</param>
    /// <returns>Valor da chave de configuração.</returns>
    public static string GetValueStringConfiguration(IConfiguration? configuration, string configurationName) =>
        SmartCoreHub.Core.SDK.Domain.Helpers.ConfigurationAppSettingsHelper.GetValueStringConfiguration(configuration, configurationName);

    /// <summary>Obtém a connection string do banco MySQL.</summary>
    /// <param name="configuration">Gerenciador de configuração.</param>
    /// <returns>String de conexão do MySQL.</returns>
    public static string GetConnectionStringMySQL(IConfiguration? configuration) =>
        SmartCoreHub.Core.SDK.Domain.Helpers.ConfigurationAppSettingsHelper.GetConnectionStringMySQL(configuration);

    /// <summary>Obtém a seção de configuração do RAG.</summary>
    /// <param name="configuration">Gerenciador de configuração.</param>
    /// <returns>Seção de configuração do RAG.</returns>
    public static IConfiguration GetRagConfig(IConfiguration configuration) =>
        SmartCoreHub.Core.SDK.Domain.Helpers.ConfigurationAppSettingsHelper.GetRagConfig(configuration);

    /// <summary>Obtém a seção de configuração de tokens JWT.</summary>
    /// <param name="configuration">Gerenciador de configuração.</param>
    /// <returns>Seção de configuração de tokens.</returns>
    public static IConfiguration GetTokenConfigurations(IConfiguration? configuration) =>
        SmartCoreHub.Core.SDK.Domain.Helpers.ConfigurationAppSettingsHelper.GetTokenConfigurations(configuration);

    /// <summary>Obtém a seção de configuração do Azure AD / Microsoft Entra ID.</summary>
    /// <param name="configuration">Gerenciador de configuração.</param>
    /// <returns>Seção de configuração do Azure AD.</returns>
    public static IConfiguration GetAzureAdConfig(IConfiguration configuration) =>
        SmartCoreHub.Core.SDK.Domain.Helpers.ConfigurationAppSettingsHelper.GetAzureAdConfig(configuration);
}
#endif
