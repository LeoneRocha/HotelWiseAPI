using HotelWise.Core.SDK.AI.Enums;
using HotelWise.Service.Prompts;

namespace HotelWise.Service.Tests.Bussines;

public class StayMatePromptGeneratorTests
{
    [Fact]
    public void CreateHotelAgentPrompt_Should_Return_NonEmpty_Agent_Prompt()
    {
        var prompt = StayMatePromptGenerator.CreateHotelAgentPrompt();

        prompt.Should().NotBeNull();
        prompt.RoleType.Should().Be(RoleAiPromptsType.Agent);
        prompt.Content.Should().NotBeNullOrWhiteSpace();
        prompt.Content.Should().Contain("StayMate");
    }

    [Fact]
    public void CreateHotelSystemPrompt_Should_Return_NonEmpty_System_Prompt()
    {
        var prompt = StayMatePromptGenerator.CreateHotelSystemPrompt();

        prompt.Should().NotBeNull();
        prompt.RoleType.Should().Be(RoleAiPromptsType.System);
        prompt.Content.Should().NotBeNullOrWhiteSpace();
        prompt.Content.Should().Contain("StayMate");
    }
}
