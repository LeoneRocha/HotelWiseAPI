using HotelWise.API.Configure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace HotelWise.API.Tests.Configure;

public class ApiDependencyInjectionTests
{
    // Cenário: Configuração de segurança da API via ServiceCollectionConfigureSecurity.
    // Objetivo: Cobrir ServiceCollectionConfigureSecurity.Configure com esquemas JWT Bearer e AzureAd.
    [Fact]
    public void ServiceCollectionConfigureSecurity_Configure_ShouldRegisterAuthSchemes()
    {
        // Arrange
        var services = new ServiceCollection();
        var tokenConfig = new TokenConfigurationDto
        {
            Audience = "hotelwise-api",
            Issuer = "hotelwise-auth",
            Secret = "SuperSecretKeyForHotelWiseTests1234567890!",
            Minutes = 60,
            DaysToExpiry = 7
        };

        var azureConfig = new AzureAdConfig
        {
            Instance = "https://login.microsoftonline.com/",
            Domain = "hotelwise.onmicrosoft.com",
            TenantId = Guid.NewGuid().ToString(),
            ClientId = Guid.NewGuid().ToString(),
            Audience = "api://hotelwise",
            ClientSecret = "secret"
        };

        var inMemorySettings = new Dictionary<string, string?>
        {
            { "AzureAd:Instance", azureConfig.Instance },
            { "AzureAd:Domain", azureConfig.Domain },
            { "AzureAd:TenantId", azureConfig.TenantId },
            { "AzureAd:ClientId", azureConfig.ClientId }
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        // Act
        Action act = () => ServiceCollectionConfigureSecurity.Configure(services, tokenConfig, configuration, azureConfig);

        // Assert
        act.Should().NotThrow();
        services.Should().Contain(d => d.ServiceType.Name.Contains("Authentication") || d.ServiceType.Name.Contains("Authorization"));
    }

    // Cenário: Tentativa de registrar dependências de ORM sem connection string configurada.
    // Objetivo: Garantir que ServiceCollectionAddAllDependencies lance InvalidOperationException quando DBConnectionMySQL estiver ausente.
    [Fact]
    public void ServiceCollectionAddAllDependencies_WithoutConnectionString_ShouldThrowException()
    {
        // Arrange
        var services = new ServiceCollection();
        var logger = new LoggerConfiguration().CreateLogger();
        IConfiguration configuration = new ConfigurationBuilder().Build();

        // Act
        Action act = () => ServiceCollectionAddAllDependencies.Configure(services, logger, configuration);

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*Connection string MySQL ausente*");
    }
}

