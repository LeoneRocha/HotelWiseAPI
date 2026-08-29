using FluentValidation;
using HotelWise.Domain.Constants.IA;
using HotelWise.Domain.Dto.IA;

namespace HotelWise.Domain.Validator.AI
{
    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — cópia Obsolete no host (AskAssistantRequest Domain ≠ Core).
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.AI.Validation.AskAssistantRequestValidator.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_AI")]
    public class AskAssistantRequestValidator : AbstractValidator<AskAssistantRequest>
    {
        public AskAssistantRequestValidator()
        {
            RuleFor(x => x.Message)
                .NotEmpty().WithMessage("A mensagem é obrigatória.")
                .MaximumLength(ChatCompletionValidatorsConstants.MaxTextLength).WithMessage($"A mensagem não pode exceder {ChatCompletionValidatorsConstants.MaxTextLength} caracteres.");

            RuleFor(x => x.Token)
                .MaximumLength(ChatCompletionValidatorsConstants.MaxTextToken).WithMessage($"O token não pode exceder {ChatCompletionValidatorsConstants.MaxTextToken} caracteres.");
        }
    }
}
