using FluentValidation;
using HotelWise.Core.SDK.AI.Constants;
using HotelWise.Core.SDK.AI.DTO;
using HotelWise.Core.SDK.AI.Enums;
using HotelWise.Core.SDK.AI.Helpers;
using HotelWise.Core.SDK.AI.Validation;

namespace HotelWise.Core.SDK.Tests.Domain;

public class LoteD7AiValidationTests
{
    [Fact]
    public void AskAssistantRequestValidator_Should_Reject_Empty_Message()
    {
        var validator = new AskAssistantRequestValidator();
        var result = validator.Validate(new AskAssistantRequest { Message = "" });
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void AskAssistantRequestValidator_Should_Accept_Valid_Message()
    {
        var validator = new AskAssistantRequestValidator();
        var result = validator.Validate(new AskAssistantRequest { Message = "Olá" });
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void PromptMessageValidator_Should_Require_Content_For_User()
    {
        var validator = new PromptMessageValidator();
        var result = validator.Validate(new PromptMessageVO
        {
            RoleType = RoleAiPromptsType.User,
            Content = ""
        });
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void HistoryPromptsValidator_Should_Reject_Empty_History()
    {
        var validator = new HistoryPromptsValidator();
        var result = validator.Validate(Array.Empty<PromptMessageVO>());
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void ChatSessionHelper_Should_Build_Context()
    {
        var history = new[]
        {
            new PromptMessageVO { RoleType = RoleAiPromptsType.User, Content = "Hi" },
            new PromptMessageVO { RoleType = RoleAiPromptsType.Assistant, Content = "Hello" }
        };

        var context = ChatSessionHelper.GenerateContextMessage(history);
        context.Should().Contain("User: Hi");
        context.Should().Contain("Assistant: Hello");

        ChatSessionHelper.GetHistoryContext(history).Should().Contain("Hi");
    }

    [Fact]
    public void TokenCounterHelper_Should_Count_From_Prompt()
    {
        var prompt = new PromptMessageVO
        {
            Content = "abcd",
            DataContextRag = new[] { new DataVectorVO { DataVector = "xxxx" } }
        };

        TokenCounterHelper.CountTokensFromPrompt(prompt).Should().Be(1 + 4);
        ChatCompletionValidatorsConstants.MaximumMessages.Should().Be(10);
    }
}
