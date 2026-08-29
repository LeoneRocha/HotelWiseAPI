using HotelWise.Domain.Dto.IA.SemanticKernel;
using HotelWise.Service.Bussines;
using HotelWise.Service.Prompts;

namespace HotelWise.Service.Tests.Bussines;

public class BusinessServicesDeepTests
{
    // Cenário: Processamento de resposta Markdown com IDs de hotéis ocultos.
    // Objetivo: Cobrir HotelResponseProcessor.ProcessResponse com matches válidos, inválidos e sem matches.
    [Fact]
    public void HotelResponseProcessor_ProcessResponse_ShouldExtractValidHotelIds()
    {
        // Arrange
        var markdown = @"
            Aqui estão suas opções:
            <!-- ID-Hotel: 101 -->
            <!-- ID-Hotel: 202 -->
            <!-- ID-Hotel: abc -->
            <!-- outro comentário -->
            Aproveite sua estadia!
        ";

        // Act
        var result = HotelResponseProcessor.ProcessResponse(markdown);
        var emptyResult = HotelResponseProcessor.ProcessResponse("Nenhum comentário aqui");

        // Assert
        Assert.Multiple(() =>
        {
            result.Should().HaveCount(2);
            result[0].Id.Should().Be(101);
            result[0].IdType.Should().Be("Hotel");
            result[1].Id.Should().Be(202);
            emptyResult.Should().BeEmpty();
        });
    }

    // Cenário: Geração de prompts StayMate (Agent e System).
    // Objetivo: Cobrir StayMatePromptGenerator.CreateHotelAgentPrompt e CreateHotelSystemPrompt.
    [Fact]
    public void StayMatePromptGenerator_ShouldGenerateAgentAndSystemPrompts()
    {
        // Act
        var agentPrompt = StayMatePromptGenerator.CreateHotelAgentPrompt();
        var systemPrompt = StayMatePromptGenerator.CreateHotelSystemPrompt();

        // Assert
        Assert.Multiple(() =>
        {
            agentPrompt.Should().NotBeNull();
            agentPrompt.Content.Should().Contain("StayMate");
            agentPrompt.RoleType.Should().Be(RoleAiPromptsType.Agent);

            systemPrompt.Should().NotBeNull();
            systemPrompt.Content.Should().Contain("StayMate");
            systemPrompt.RoleType.Should().Be(RoleAiPromptsType.System);
        });
    }
}
