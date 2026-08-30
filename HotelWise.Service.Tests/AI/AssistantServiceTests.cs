using AutoMapper;
using HotelWise.Domain.Dto.IA;
using HotelWise.Domain.Interfaces.Entity.IA;
using HotelWise.Domain.Model.AI;
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

        var sut = CreateSut();
        sut.SetUserId(123);
        var result = await sut.AskAssistant(request);

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

    [Fact]
    public async Task AskAssistant_WithExistingSession_ShouldUpdateSessionAndReturnResponses()
    {
        var existingSession = new ChatSessionHistoryDto
        {
            IdToken = "session-token-2",
            Title = "Histórico anterior",
            PromptMessageHistory =
            [
                new PromptMessageVO { RoleType = RoleAiPromptsType.User, Content = "<p>Mensagem anterior</p>" },
                new PromptMessageVO { RoleType = RoleAiPromptsType.Assistant, Content = "Resposta anterior" }
            ]
        };

        _chatSession.Setup(s => s.GetByIdTokenAsync("session-token-2"))
            .ReturnsAsync(existingSession);
        _chatSession.Setup(s => s.UpdateAsync(It.IsAny<ChatSessionHistoryDto>()))
            .ReturnsAsync(new ServiceResponse<ChatSessionHistoryDto> { Success = true });

        _mapper.Setup(m => m.Map<ChatSessionHistoryDto>(It.IsAny<ChatSessionHistory>()))
            .Returns(existingSession);

        _inference.Setup(i => i.GenerateChatCompletionByAgentAsync(
                It.IsAny<PromptMessageVO[]>(),
                InferenceAiAdapterType.SemanticKernel))
            .ReturnsAsync("Aqui estão as recomendações adicionais.");

        var request = new AskAssistantRequest
        {
            Message = "Esta é uma mensagem muito longa que possui mais de cinquenta caracteres para testar o título truncado no CreateNewSession caso aplicável",
            Token = "session-token-2"
        };

        var sut = CreateSut();
        var result = await sut.AskAssistant(request);

        result.Should().NotBeNull();
        result![0].Message.Should().Contain("recomendações");
        _chatSession.Verify(s => s.UpdateAsync(It.IsAny<ChatSessionHistoryDto>()), Times.Once);
    }

    [Fact]
    public async Task AskAssistant_WhenValidationFailsOrExceptionOccurs_ShouldReturnNull()
    {
        var request = new AskAssistantRequest
        {
            Message = "", // Mensagem vazia -> Falha no AskAssistantRequestValidator
            Token = "invalid-token"
        };

        var sut = CreateSut();
        var result = await sut.AskAssistant(request);

        result.Should().BeNull();
    }

    [Fact]
    public async Task GenerateEmbeddingAsync_ShouldCallInferenceService()
    {
        _inference.Setup(i => i.GenerateEmbeddingAsync("Texto de teste", InferenceAiAdapterType.SemanticKernel))
            .ReturnsAsync([0.1f, 0.2f, 0.3f]);

        var sut = CreateSut();
        var result = await sut.GenerateEmbeddingAsync("Texto de teste");

        result.Should().NotBeNull();
        result.Should().HaveCount(3);
    }
}

