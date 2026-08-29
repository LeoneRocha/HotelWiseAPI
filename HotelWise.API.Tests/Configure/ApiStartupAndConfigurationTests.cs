using HotelWise.API.Configure;
using HotelWise.Data.Context;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace HotelWise.API.Tests.Configure;

public class ApiStartupAndConfigurationTests
{
    // Cenário: Configuração completa da coleção de serviços da API e resolução de opções/DI.
    // Objetivo: Cobrir WebApplicationConfigureServiceCollections.Configure, lambdas de Swagger, Mvc, DBContext e ServiceCollectionAddAllDependencies.
    [Fact]
    public void WebApplicationConfigureServiceCollections_Configure_ShouldRegisterAllServices()
    {
        // Arrange
        var services = new ServiceCollection();
        var logger = new LoggerConfiguration().CreateLogger();

        var inMemorySettings = new Dictionary<string, string?>
        {
            { "ConnectionStrings:DBConnectionMySQL", "Server=localhost;Database=hotelwisedb;Uid=root;Pwd=root;" },
            { "ApplicationInsights:ConnectionString", "InstrumentationKey=00000000-0000-0000-0000-000000000000;" },
            { "TokenConfigurations:Audience", "hotelwise-client" },
            { "TokenConfigurations:Issuer", "hotelwise-auth" },
            { "TokenConfigurations:Seconds", "3600" },
            { "TokenConfigurations:Secret", "SuperSecretKeyForHotelWiseTests1234567890!" },
            { "AzureAd:Instance", "https://login.microsoftonline.com/" },
            { "AzureAd:Domain", "hotelwise.com" },
            { "AzureAd:TenantId", "tenant-id" },
            { "AzureAd:ClientId", "client-id" },
            { "ApplicationIAConfig:DefaultInferenceAdapter", "GroqApi" },
            { "ApplicationIAConfig:Rag:VectorStoreType", "InMemory" },
            { "ApplicationIAConfig:Rag:AIChatServiceApi", "MistralApi" },
            { "ApplicationIAConfig:Rag:AIEmbeddingServiceApi", "MistralApiEmbeddings" },
            { "ApplicationIAConfig:Rag:AIChatServiceAdapter", "SemanticKernel" },
            { "ApplicationIAConfig:Rag:AIEmbeddingServiceApiAdapter", "SemanticKernel" },
            { "ApplicationIAConfig:Rag:BuildCollection", "true" },
            { "ApplicationIAConfig:Rag:VectorStoreCollectionPrefixName", "test_" },
            { "ApplicationIAConfig:Rag:VectorStoreDimensions", "1024" },
            { "ApplicationIAConfig:Rag:DataLoadingBatchSize", "10" },
            { "ApplicationIAConfig:Rag:DataLoadingBetweenBatchDelayInMilliseconds", "100" },
            { "ApplicationIAConfig:VectorStores:AzureAISearch:Endpoint", "https://test.search.windows.net" },
            { "ApplicationIAConfig:VectorStores:AzureCosmosDBMongoDB:ConnectionString", "mongodb://localhost" },
            { "ApplicationIAConfig:VectorStores:AzureCosmosDBNoSQL:ConnectionString", "AccountEndpoint=https://localhost" },
            { "ApplicationIAConfig:VectorStores:Qdrant:Host", "localhost" },
            { "ApplicationIAConfig:VectorStores:Redis:ConnectionConfiguration", "localhost:6379" },
            { "ApplicationIAConfig:VectorStores:Weaviate:Endpoint", "http://localhost:8080" },
            { "ApplicationIAConfig:AIServices:AzureOpenAI:Endpoint", "https://test.openai.azure.com" },
            { "ApplicationIAConfig:AIServices:AzureOpenAI:ChatDeploymentName", "gpt-4" },
            { "ApplicationIAConfig:AIServices:AzureOpenAIEmbeddings:Endpoint", "https://test.openai.azure.com" },
            { "ApplicationIAConfig:AIServices:AzureOpenAIEmbeddings:DeploymentName", "text-embedding-ada-002" },
            { "ApplicationIAConfig:AIServices:OpenAI:ApiKey", "test-key" },
            { "ApplicationIAConfig:AIServices:OpenAI:ModelId", "gpt-4o" },
            { "ApplicationIAConfig:AIServices:OpenAIEmbeddings:ApiKey", "test-key" },
            { "ApplicationIAConfig:AIServices:OpenAIEmbeddings:ModelId", "text-embedding-3-small" },
            { "ApplicationIAConfig:AIServices:GroqApi:ApiKey", "test-key" },
            { "ApplicationIAConfig:AIServices:GroqApi:ModelId", "llama3-70b-8192" },
            { "ApplicationIAConfig:AIServices:MistralApi:ApiKey", "test-key" },
            { "ApplicationIAConfig:AIServices:MistralApi:ModelId", "mistral-large-latest" },
            { "ApplicationIAConfig:AIServices:MistralApiEmbeddings:ApiKey", "test-key" },
            { "ApplicationIAConfig:AIServices:MistralApiEmbeddings:ModelId", "mistral-embed" },
            { "ApplicationIAConfig:AIServices:OllamaApi:Endpoint", "http://localhost:11434" },
            { "ApplicationIAConfig:AIServices:OllamaApi:ModelId", "llama3.2" },
            { "ApplicationIAConfig:AIServices:OllamaApi:EndpointEmbeddings", "http://localhost:11434" },
            { "ApplicationIAConfig:AIServices:OllamaApi:ModelIdEmbeddings", "nomic-embed-text" }
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        // Act
        WebApplicationConfigureServiceCollections.Configure(services, configuration, logger);

        var serviceProvider = services.BuildServiceProvider();

        // Executar lambdas de configuração registrados
        var swaggerOptions = serviceProvider.GetService<IOptions<SwaggerGenOptions>>()?.Value;
        var mvcOptions = serviceProvider.GetService<IOptions<MvcOptions>>()?.Value;
        var serilogLogger = serviceProvider.GetService<Serilog.ILogger>();
        var dbContextFactory = serviceProvider.GetService<IDbContextFactory<HotelWiseDbContextMysql>>();

        // Assert
        Assert.Multiple(() =>
        {
            swaggerOptions.Should().NotBeNull();
            mvcOptions.Should().NotBeNull();
            serilogLogger.Should().NotBeNull();
            dbContextFactory.Should().NotBeNull();
        });
    }

    // Cenário: Execução dos eventos de autenticação JWT e AzureAD configurados em ServiceCollectionConfigureSecurity.
    // Objetivo: Cobrir OnAuthenticationFailed e OnTokenValidated.
    [Fact]
    public async Task ServiceCollectionConfigureSecurity_Events_ShouldExecuteSuccessfully()
    {
        // Arrange
        var services = new ServiceCollection();
        var loggerMock = new Mock<Serilog.ILogger>();
        services.AddSingleton(loggerMock.Object);

        var tokenConfig = new TokenConfigurationDto
        {
            Audience = "hotelwise-client",
            Issuer = "hotelwise-auth",
            Secret = "SuperSecretKeyForHotelWiseTests1234567890!",
            Minutes = 60,
            DaysToExpiry = 7
        };

        var azureConfig = new AzureAdConfig
        {
            Instance = "https://login.microsoftonline.com/",
            Domain = "hotelwise.com",
            TenantId = "tenant-id",
            ClientId = "client-id"
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

        services.AddSingleton(configuration);

        ServiceCollectionConfigureSecurity.Configure(services, tokenConfig, configuration, azureConfig);
        var serviceProvider = services.BuildServiceProvider();

        var jwtOptions = serviceProvider.GetRequiredService<IOptionsMonitor<JwtBearerOptions>>().Get(JwtBearerDefaults.AuthenticationScheme);
        var azureAdOptions = serviceProvider.GetRequiredService<IOptionsMonitor<JwtBearerOptions>>().Get("AzureAd");

        var httpContext = new DefaultHttpContext { RequestServices = serviceProvider };
        var authScheme = new AuthenticationScheme(JwtBearerDefaults.AuthenticationScheme, null, typeof(JwtBearerHandler));

        // Act & Assert para JwtBearer
        if (jwtOptions.Events != null)
        {
            var authFailedCtx = new AuthenticationFailedContext(httpContext, authScheme, jwtOptions) { Exception = new Exception("JWT Failed") };
            await jwtOptions.Events.AuthenticationFailed(authFailedCtx);

            var tokenValidatedCtx = new TokenValidatedContext(httpContext, authScheme, jwtOptions)
            {
                Principal = new System.Security.Claims.ClaimsPrincipal(new System.Security.Claims.ClaimsIdentity("Test"))
            };
            await jwtOptions.Events.TokenValidated(tokenValidatedCtx);
        }

        // Act & Assert para AzureAD
        if (azureAdOptions.Events != null)
        {
            var azureFailedCtx = new AuthenticationFailedContext(httpContext, authScheme, azureAdOptions) { Exception = new Exception("Azure Failed") };
            await azureAdOptions.Events.AuthenticationFailed(azureFailedCtx);

            var azureValidatedCtx = new TokenValidatedContext(httpContext, authScheme, azureAdOptions)
            {
                Principal = new System.Security.Claims.ClaimsPrincipal(new System.Security.Claims.ClaimsIdentity("Test"))
            };
            await azureAdOptions.Events.TokenValidated(azureValidatedCtx);
        }

        var authOptions = serviceProvider.GetService<IOptions<Microsoft.AspNetCore.Authorization.AuthorizationOptions>>()?.Value;
        var bearerPolicy = authOptions?.GetPolicy("Bearer");
        var azurePolicy = authOptions?.GetPolicy("AzureAd");

        bearerPolicy.Should().NotBeNull();
        azurePolicy.Should().NotBeNull();

        loggerMock.Verify(l => l.Error(It.IsAny<string>(), It.IsAny<Exception>()), Times.AtLeastOnce);
        loggerMock.Verify(l => l.Information(It.IsAny<string>()), Times.AtLeastOnce);
    }

    // Cenário: Tentativa de BuildAndRunAPP com logger nulo.
    // Objetivo: Garantir que lance InvalidOperationException.
    [Fact]
    public void WebApplicationConfigureBuilder_BuildAndRunAPP_WithNullLogger_ShouldThrowException()
    {
        // Arrange
        var builder = WebApplication.CreateBuilder();

        // Act
        Action act = () => WebApplicationConfigureBuilder.BuildAndRunAPP(builder, null);

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*Logger Serilog não foi inicializado*");
    }
}
