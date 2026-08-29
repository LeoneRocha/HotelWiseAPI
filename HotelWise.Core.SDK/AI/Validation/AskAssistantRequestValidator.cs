#if NET8_0_OR_GREATER
using FluentValidation;
using HotelWise.Core.SDK.AI.Constants;
using HotelWise.Core.SDK.AI.DTO;

namespace HotelWise.Core.SDK.AI.Validation;

/// <summary>
/// Valida solicitações ao assistente conversacional (<see cref="AskAssistantRequest"/>).
/// Garante mensagem obrigatória e limites de comprimento para mensagem e token de sessão.
/// </summary>
public class AskAssistantRequestValidator : AbstractValidator<AskAssistantRequest>
{
    /// <summary>
    /// Inicializa as regras de validação da solicitação ao assistente.
    /// </summary>
    public AskAssistantRequestValidator()
    {
        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("A mensagem é obrigatória.")
            .MaximumLength(ChatCompletionValidatorsConstants.MaxTextLength)
            .WithMessage($"A mensagem não pode exceder {ChatCompletionValidatorsConstants.MaxTextLength} caracteres.");

        RuleFor(x => x.Token)
            .MaximumLength(ChatCompletionValidatorsConstants.MaxTextToken)
            .WithMessage($"O token não pode exceder {ChatCompletionValidatorsConstants.MaxTextToken} caracteres.");
    }
}
#endif
