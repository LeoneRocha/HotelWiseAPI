using HotelWise.Core.SDK.AI.Abstractions;
using HotelWise.Core.SDK.AI.Constants;
using HotelWise.Core.SDK.AI.DTO;
using HotelWise.Core.SDK.AI.Helpers;

namespace HotelWise.Core.SDK.Tests.Domain;

public class LoteD5AiAbstractionsTests
{
    [Fact]
    public void Enums_Should_Expose_Expected_Values()
    {
        AIChatServiceType.GroqApi.ToString().Should().Be("GroqApi");
        InferenceAiAdapterType.SemanticKernel.ToString().Should().Be("SemanticKernel");
        VectorStoreType.Qdrant.ToString().Should().Be("Qdrant");
    }

    [Fact]
    public void ChatCompletionValidatorsConstants_Should_Expose_Limits()
    {
        ChatCompletionValidatorsConstants.MaximumMessages.Should().Be(10);
        ChatCompletionValidatorsConstants.MaxTextLength.Should().Be(2500);
    }

    [Fact]
    public void TokenCounterHelper_Should_Approximate_Tokens()
    {
        TokenCounterHelper.CountTokens("abcd").Should().Be(1);
    }

    [Fact]
    public void PromptMessageVO_Should_Expose_Role_Description()
    {
        var msg = new PromptMessageVO { RoleType = RoleAiPromptsType.User, Content = "hello" };
        msg.Role.Should().Be("user");
        msg.ContentLenght.Should().Be(5);
    }

    [Fact]
    public void IDataVector_Contract_Should_Be_Interface()
    {
        typeof(IDataVector).IsInterface.Should().BeTrue();
        typeof(IAIInferenceAdapter).IsInterface.Should().BeTrue();
        typeof(IVectorStoreAdapter<>).IsGenericTypeDefinition.Should().BeTrue();
    }
}
