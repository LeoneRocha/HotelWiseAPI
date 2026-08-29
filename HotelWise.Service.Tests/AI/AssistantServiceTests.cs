using AutoMapper;
using HotelWise.Core.SDK.AI.Abstractions;
using HotelWise.Core.SDK.AI.Configuration;
using HotelWise.Core.SDK.AI.DTO;
using HotelWise.Core.SDK.AI.Enums;
using HotelWise.Core.SDK.Common;
using HotelWise.Domain.Dto.IA;
using HotelWise.Domain.Interfaces.Entity.IA;
using HotelWise.Service.Entity;

namespace HotelWise.Service.Tests.AI;

public class AssistantServiceTests
{
    private readonly Mock<Serilog.ILogger> _logger = new();
    private readonly Mock<IApplicationIAConfig> _appConfig = new();
    private readonly Mock<IAIInferenceService> _inference = new();
    private readonly Mock<IChatSessionHistoryService> _chatSession = new();
    private readonly Mock<IMapper> _mapper = new();

    public AssistantServiceTests()
    {
        _appConfig.SetupGet(c => c.RagConfig).Returns(new RagConfig
        {
            AIChatServiceAdapter = AIChatServiceType.SemanticKernel
        });
    }

    private AssistantService CreateSut() =>
        new(_logger.Object, _appConfig.Object, _inference.Object, _chatSession.Object, _mapper.Object);

    [Fact]
    public async Task AskAssistant_Should_Return_Responses_On_Happy_Path()
    {
        _chatSession.Setup(s => s.GetByIdTokenAsync(It.IsAny<string>()))
            .ReturnsAsync((ChatSessionHistoryDto?)null);
        _chatSession.Setup(s => s.CreateAsync(It.IsAny<ChatSessionHistoryDto>()))
            .ReturnsAsync(new ServiceResponse<ChatSessionHistoryDto> { Success = true });

        _inference.Setup(i => i.GenerateChatCompletionByAgentAsync(
                It.IsAny<PromptMessageVO[]>(),
                InferenceAiAdapterType.SemanticKernel))
            .ReturnsAsync("Olá! Posso ajudar com sua viagem.");

        var request = new AskAssistantRequest
        {
            Message = "Quais hotéis em Lisboa?",
            Token = "session-token-1"
        };

        var result = await CreateSut().AskAssistant(request);

        result.Should().NotBeNull();
        result.Should().ContainSingle();
        result![0].Message.Should().Be("Olá! Posso ajudar com sua viagem.");
        result[0].Role.Should().Be(RoleAiPromptsType.Assistant);
        result[0].Token.Should().Be("session-token-1");
        _inference.Verify(i => i.GenerateChatCompletionByAgentAsync(
            It.IsAny<PromptMessageVO[]>(),
            InferenceAiAdapterType.SemanticKernel), Times.Once);
        _chatSession.Verify(s => s.CreateAsync(It.IsAny<ChatSessionHistoryDto>()), Times.Once);
    }
}
