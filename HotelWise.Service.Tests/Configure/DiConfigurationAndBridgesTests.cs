using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HotelWise.Service.Tests.Configure;

public class DiConfigurationAndBridgesTests
{
    // Cenário: Registro de serviços de IA do host e genéricos.
    // Objetivo: Cobrir ConfigureServicesAI.ConfigureServices e verificar injeção de IAssistantService e IGenerateHotelService.
    [Fact]
    public void ConfigureServicesAI_ConfigureServices_ShouldRegisterExpectedServices()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        HotelWise.Service.Configure.ConfigureServicesAI.ConfigureServices(services);

        // Assert
        Assert.Multiple(() =>
        {
            services.Should().Contain(d => d.ServiceType == typeof(IGenerateHotelService));
            services.Should().Contain(d => d.ServiceType == typeof(IAssistantService));
        });
    }

    // Cenário: Configuração de AutoMapper via ServiceCollectionConfigureAutoMapper.
    // Objetivo: Cobrir ServiceCollectionConfigureAutoMapper.Configure.
    [Fact]
    public void ServiceCollectionConfigureAutoMapper_Configure_ShouldNotThrow()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        Action act = () => HotelWise.Service.Configure.ServiceCollectionConfigureAutoMapper.Configure(services);

        // Assert
        act.Should().NotThrow();
    }

    // Cenário: Configuração de CORS via ServiceCollectionConfigureCors.
    // Objetivo: Cobrir ServiceCollectionConfigureCors.Configure.
    [Fact]
    public void ServiceCollectionConfigureCors_Configure_ShouldRegisterCors()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        HotelWise.Service.Configure.ServiceCollectionConfigureCors.Configure(services);

        // Assert
        services.Should().Contain(d => d.ServiceType.Name.Contains("Cors"));
    }

    // Cenário: Registro automático de repositórios e serviços de domínio.
    // Objetivo: Cobrir ServicesDomainRepository.AddDependenciesAuto e ServicesDomainService.AddDependenciesAuto.
    [Fact]
    public void ServicesDomain_AddDependenciesAuto_ShouldRegisterDomainComponents()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        HotelWise.Service.Configure.ServicesDomainRepository.AddDependenciesManually(services);
        HotelWise.Service.Configure.ServicesDomainService.AddDependenciesManually(services);
        HotelWise.Service.Configure.ServicesDomainRepository.AddDependenciesAuto(services);
        HotelWise.Service.Configure.ServicesDomainService.AddDependenciesAuto(services);

        // Assert
        services.Should().NotBeEmpty();
    }

    // Cenário: Configuração completa via ServiceCollectionConfigureServicesDomain.
    // Objetivo: Cobrir ServiceCollectionConfigureServicesDomain.Configure.
    [Fact]
    public void ServiceCollectionConfigureServicesDomain_Configure_ShouldRunSuccessfully()
    {
        // Arrange
        var services = new ServiceCollection();
        var inMemorySettings = new Dictionary<string, string?>
        {
            { "TokenConfigurations:Audience", "hotelwise-client" },
            { "TokenConfigurations:Issuer", "hotelwise-auth" },
            { "TokenConfigurations:Seconds", "3600" },
            { "TokenConfigurations:Secret", "SuperSecretKeyForHotelWiseTests1234567890!" },
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
        Action act = () => HotelWise.Service.Configure.ServiceCollectionConfigureServicesDomain.Configure(services, configuration);

        // Assert
        act.Should().NotThrow();
    }
}
