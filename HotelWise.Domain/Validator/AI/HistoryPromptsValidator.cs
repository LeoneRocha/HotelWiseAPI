using FluentValidation;
using HotelWise.Domain.Constants.IA;
using HotelWise.Domain.Dto.IA.SemanticKernel;

namespace HotelWise.Domain.Validator.AI
{
    /// <summary>
    /// ⚠️ Movido para HotelWise.Core.SDK — cópia Obsolete no host (PromptMessageVO Domain ≠ Core).
    /// </summary>
    [Obsolete(
        "Movido para HotelWise.Core.SDK. Use HotelWise.Core.SDK.AI.Validation.HistoryPromptsValidator.",
        error: false,
        DiagnosticId = "HW_CORE_SDK_AI")]
    public class HistoryPromptsValidator : AbstractValidator<PromptMessageVO[]>
    {
        public HistoryPromptsValidator()
        {
            RuleFor(x => x.Length)
                .GreaterThan(0).WithMessage("O histórico de prompts não pode estar vazio.")
                .LessThanOrEqualTo(ChatCompletionValidatorsConstants.MaximumMessages).WithMessage("O histórico de prompts não pode conter mais de 10 mensagens.");

            RuleForEach(x => x).SetValidator(new PromptMessageValidator());

            RuleFor(x => x)
                .Must(NotExceedMaxTokens).WithMessage($"A soma total de tokens no histórico não pode exceder {ChatCompletionValidatorsConstants.MaxTotalTokens}.");
        }

        private static bool NotExceedMaxTokens(PromptMessageVO[] prompts)
        {
            if (prompts == null || prompts.Length == 0) return true;
            return prompts.Sum(p => p.TokenCount) <= ChatCompletionValidatorsConstants.MaxTotalTokens;
        }
    }
}
