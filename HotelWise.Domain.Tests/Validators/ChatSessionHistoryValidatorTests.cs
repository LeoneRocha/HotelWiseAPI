using FluentValidation;
using HotelWise.Core.SDK.AI.DTO;
using HotelWise.Core.SDK.AI.Enums;
using HotelWise.Domain.Model.AI;
using HotelWise.Domain.Validator.AI;

namespace HotelWise.Domain.Tests.Validators;

public class ChatSessionHistoryValidatorTests
{
    private readonly ChatSessionHistoryValidator _validator = new();

    // Cenário: sessão com IdToken GUID e histórico de mensagens preenchido
    // Objetivo: garantir que a validação passa para um histórico válido
    [Fact]
    public async Task ValidateAsync_ValidWithGuidIdToken_Passes()
    {
        // Arrange
        var history = new ChatSessionHistory
        {
            Title = "Busca de hotéis",
            IdToken = Guid.NewGuid().ToString(),
            PromptMessageHistory =
            [
                new PromptMessageVO
                {
                    RoleType = RoleAiPromptsType.User,
                    Content = "Quero um hotel em SP"
                }
            ],
            CountMessages = 1,
            TotalTokensMessage = 10,
            SessionDateTime = DateTime.UtcNow,
            IdUser = 1
        };

        // Act
        var result = await _validator.ValidateAsync(history);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    // Cenário: IdToken que não é um GUID válido
    // Objetivo: garantir que Must(BeAValidGuid) rejeita tokens inválidos
    [Fact]
    public async Task ValidateAsync_InvalidGuidIdToken_Fails()
    {
        // Arrange
        var history = new ChatSessionHistory
        {
            Title = "Busca de hotéis",
            IdToken = "not-a-guid",
            PromptMessageHistory =
            [
                new PromptMessageVO
                {
                    RoleType = RoleAiPromptsType.User,
                    Content = "Olá"
                }
            ],
            CountMessages = 1,
            TotalTokensMessage = 5,
            SessionDateTime = DateTime.UtcNow
        };

        // Act
        var result = await _validator.ValidateAsync(history);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(ChatSessionHistory.IdToken));
    }
}
