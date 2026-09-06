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
    public void CreateHotelSystemPrompt_Should_Delegate_To_Canonical_Agent_Prompt()
    {
#pragma warning disable CS0618 // Obsolete alias retained for compatibility
        var prompt = StayMatePromptGenerator.CreateHotelSystemPrompt();
#pragma warning restore CS0618

        prompt.Should().NotBeNull();
        prompt.RoleType.Should().Be(RoleAiPromptsType.Agent);
        prompt.Content.Should().NotBeNullOrWhiteSpace();
        prompt.Content.Should().Contain("StayMate");
    }
}
