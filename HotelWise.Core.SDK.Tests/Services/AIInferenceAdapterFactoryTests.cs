using HotelWise.Core.SDK.AI.Abstractions;
using HotelWise.Core.SDK.AI.Enums;
using HotelWise.Core.SDK.AI.Services;
using HotelWise.Core.SDK.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace HotelWise.Core.SDK.Tests.Services;

public class AIInferenceAdapterFactoryTests
{
    [Theory]
    [InlineData(InferenceAiAdapterType.GroqApi, typeof(HotelWise.Core.SDK.AI.Adapters.GroqApiAdapter))]
    [InlineData(InferenceAiAdapterType.Mistral, typeof(HotelWise.Core.SDK.AI.Adapters.MistralApiAdapter))]
    [InlineData(InferenceAiAdapterType.Ollama, typeof(HotelWise.Core.SDK.AI.Adapters.OllamaAdapter))]
    public void CreateAdapter_Should_Return_Expected_Type(InferenceAiAdapterType type, Type expected)
    {
        var config = new Mock<IApplicationIAConfig>();
        config.SetupGet(c => c.GroqApiConfig).Returns(new HotelWise.Core.SDK.AI.Configuration.GroqApiConfig
        {
            ApiKey = "k",
            ModelId = "m"
        });
        config.SetupGet(c => c.MistralApiConfig).Returns(new HotelWise.Core.SDK.AI.Configuration.MistralApiConfig
        {
            ApiKey = "k",
            ModelId = "m"
        });
        config.Setup(c => c.GetChatServiceConfig(AIChatServiceType.OllamaAdapter))
            .Returns(new HotelWise.Core.SDK.AI.Configuration.OllamaConfig
            {
                Endpoint = "http://localhost:11434",
                ModelId = "llama"
            });

        var factory = new AIInferenceAdapterFactory(config.Object, Mock.Of<IServiceProvider>());
        var adapter = factory.CreateAdapter(type);
        adapter.Should().BeOfType(expected);
    }
}

public class DiExtensionsSmokeTests
{
    [Fact]
    public void Cors_And_AutoMapper_Helpers_Should_Register()
    {
        var services = new ServiceCollection();
        ServiceCollectionConfigureCors.Configure(services);
        ServiceCollectionConfigureAutoMapper.AddProfile<DummyProfile>(services);
        services.Should().NotBeEmpty();
    }

    private sealed class DummyProfile : AutoMapper.Profile
    {
        public DummyProfile()
        {
            CreateMap<string, string>();
        }
    }
}
