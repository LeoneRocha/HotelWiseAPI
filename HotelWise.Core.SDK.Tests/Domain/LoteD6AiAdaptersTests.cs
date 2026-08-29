using HotelWise.Core.SDK.AI.Adapters;
using HotelWise.Core.SDK.AI.Configuration;
using HotelWise.Core.SDK.AI.DTO;
using HotelWise.Core.SDK.AI.Helpers;

namespace HotelWise.Core.SDK.Tests.Domain;

public class LoteD6AiAdaptersTests
{
    [Fact]
    public void EmbeddingHelper_Should_Convert_Float_Array()
    {
        var memory = EmbeddingHelper.ConvertToReadOnlyMemory(new[] { 1f, 2f, 3f });
        memory.Length.Should().Be(3);
        memory.Span[0].Should().Be(1f);
    }

    [Fact]
    public void SearchCriteria_Should_Default_Empty_Tags()
    {
        var criteria = new SearchCriteria { MaxHotelRetrieve = 5 };
        criteria.TagsCriteria.Should().BeEmpty();
        criteria.MaxHotelRetrieve.Should().Be(5);
    }

    [Fact]
    public void RagConfig_Should_Map_Adapter_Type()
    {
        var rag = new RagConfig
        {
            AIChatServiceAdapter = HotelWise.Core.SDK.AI.Enums.AIChatServiceType.GroqApi
        };
        rag.GetAInferenceAdapterType().Should().Be(HotelWise.Core.SDK.AI.Enums.InferenceAiAdapterType.GroqApi);
    }

    [Fact]
    public void Adapter_Types_Should_Exist_On_Net8OrGreater()
    {
        typeof(GroqApiAdapter).Should().NotBeNull();
        typeof(MistralApiAdapter).Should().NotBeNull();
        typeof(OllamaAdapter).Should().NotBeNull();
        typeof(SemanticKernelAdapter).Should().NotBeNull();
        typeof(GenericVectorStoreAdapter<>).IsGenericTypeDefinition.Should().BeTrue();
        typeof(DataVectorBase).IsAbstract.Should().BeTrue();
    }

    [Fact]
    public void Simple_Configs_Should_Expose_Section_Names()
    {
        GroqApiConfig.ConfigSectionName.Should().Be("GroqApi");
        OllamaConfig.ConfigSectionName.Should().Be("OllamaApi");
        QdrantConfig.ConfigSectionName.Should().Be("Qdrant");
        AzureCosmosDBConfig.MongoDBConfigSectionName.Should().Be("AzureCosmosDBMongoDB");
    }
}
