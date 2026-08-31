#if NET8_0_OR_GREATER
using System.Diagnostics;
using System.Reflection;
using System.Text;
using HotelWise.Core.SDK.Common;
using HotelWise.Core.SDK.Common.Exceptions;
using HotelWise.Core.SDK.Helpers;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace HotelWise.Core.SDK.Logging;

/// <summary>
/// Helpers de logging Serilog e informações de versão do produto.
/// Centraliza criação do logger, formatação de duração, registro de exceções
/// (Warning vs Error) e impressão de metadados do assembly de entrada.
/// </summary>
/// <example>
/// <code>
/// Log.Logger = LogAppHelper.CreateLogger(configuration);
/// LogAppHelper.PrintLogInformationVersionProduct(Log.Logger);
/// LogAppHelper.LogException(Log.Logger, ex, "API");
/// </code>
/// </example>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Service. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Service.API.Helpers.Ported.LogAppHelper. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public static class LogAppHelper
{
    /// <summary>
    /// Formata a duração de um <see cref="Stopwatch"/> como <c>hh:mm:ss</c>.
    /// </summary>
    /// <param name="stopwatch">Cronômetro com tempo decorrido.</param>
    /// <returns>Duração formatada.</returns>
    public static string GetDurationStopwatch(Stopwatch stopwatch)
    {
        return TimeSpan.FromMilliseconds(stopwatch.ElapsedMilliseconds).ToString(@"hh\:mm\:ss");
    }

    /// <summary>
    /// Registra a exceção como Warning (<see cref="AppWarningException"/>) ou Error (demais).
    /// </summary>
    /// <param name="logger">Logger Serilog.</param>
    /// <param name="ex">Exceção a registrar.</param>
    /// <param name="logType">Rótulo de origem do log (ex.: "API").</param>
    public static void LogException(ILogger logger, Exception ex, string logType)
    {
        var message = $"{logType}-LEVEL: {ex.Message} at: {DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm:ss")}";
        if (ex is AppWarningException)
        {
            message = message.Replace("-LEVEL:", "-Warning:");
            logger.Warning(message);
        }
        else
        {
            message = message.Replace("-LEVEL:", "-Error:");
            logger.Error(ex, message);
        }
    }

    /// <summary>
    /// Cria um logger Serilog a partir da configuração, com enrichers padrão do HotelWise.
    /// </summary>
    /// <param name="configuration">Configuração da aplicação (seção Serilog).</param>
    /// <returns>Instância de <see cref="Serilog.Core.Logger"/>.</returns>
    public static Serilog.Core.Logger CreateLogger(IConfiguration configuration)
    {
        return new LoggerConfiguration()
                  .ReadFrom.Configuration(configuration)
                  .Enrich.FromLogContext()
                  .Enrich.WithEnvironmentName()
                  .Enrich.WithMachineName()
                  .Enrich.WithProperty("Application", "HotelWise.API")
                  .CreateLogger();
    }

    /// <summary>
    /// Obtém nome, versão e ambiente do assembly de entrada da aplicação.
    /// </summary>
    /// <returns>DTO com metadados do produto e mensagem formatada.</returns>
    public static AppInformationVersionProductDto GetInformationVersionProduct()
    {
        var assembly = Assembly.GetEntryAssembly();
        var appDto = new AppInformationVersionProductDto() { Name = "Unknown", Version = "Unknown", EnvironmentName = "Unknown" };

        if (assembly != null)
        {
            var assemblyApp = assembly.GetName();
            if (assemblyApp != null)
            {
                var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Undefined";
                var nameApp = assemblyApp.Name ?? "Undefined";
                var version = "Undefined";
                if (assemblyApp.Version != null)
                    version = assemblyApp.Version.ToString();

                appDto.Name = nameApp;
                appDto.Version = version;
                appDto.EnvironmentName = envName;

                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("******* PRODUCT INFORMATION ******* {0}", Environment.NewLine);
                sb.AppendFormat("Name: {0} | Version: {1} | Environment: {2} {3}", appDto.Name, appDto.Version, appDto.EnvironmentName, Environment.NewLine);
                sb.AppendFormat("******* PRODUCT INFORMATION ******* {0}", Environment.NewLine);
                appDto.Message = sb.ToString();
            }
        }
        else
        {
            appDto.Message = string.Format("Assembly information could not be retrieved.{0}", Environment.NewLine);
        }
        return appDto;
    }

    /// <summary>
    /// Retorna a mensagem textual com informações de versão do produto.
    /// </summary>
    /// <returns>Bloco de texto com Name/Version/Environment ou mensagem de falha.</returns>
    public static string ShowInformationVersionProductString()
    {
        var assemblyApp = GetInformationVersionProduct();
        if (assemblyApp != null)
        {
            return assemblyApp.Message;
        }
        return "Assembly information could not be retrieved.";
    }

    /// <summary>
    /// Emite no logger as informações de versão do produto (bloco PRODUCT INFORMATION).
    /// </summary>
    /// <param name="logger">Logger Serilog de destino.</param>
    public static void PrintLogInformationVersionProduct(ILogger logger)
    {
        logger.Information("******* PRODUCT INFORMATION *******");
        var assemblyApp = GetInformationVersionProduct();
        if (assemblyApp != null)
        {
            logger.Information("Name: {Name} | Version: {Version} | Environment: {EnvName}", assemblyApp.Name, assemblyApp.Version, assemblyApp.EnvironmentName);
        }
        else
        {
            logger.Information("Assembly information could not be retrieved.");
        }
        logger.Information("******* PRODUCT INFORMATION *******");
    }

    /// <summary>
    /// Define a variável de ambiente <c>ASPNETCORE_ENVIRONMENT</c> a partir da chave APP_ENVIRONMENT.
    /// </summary>
    /// <param name="configuration">Configuração da aplicação.</param>
    public static void Set_ASPNETCORE_ENVIRONMENT(IConfiguration configuration)
    {
        string envVal = ConfigurationAppSettingsHelper.GetValueStringConfiguration(configuration, "APP_ENVIRONMENT");
        if (!string.IsNullOrEmpty(envVal))
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", envVal);
        }
    }
}
#endif
