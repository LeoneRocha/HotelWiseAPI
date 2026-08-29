#if NET8_0_OR_GREATER
using FluentValidation;
using HotelWise.Core.SDK.AI.Constants;
using HotelWise.Core.SDK.AI.DTO;

namespace HotelWise.Core.SDK.AI.Validation;

/// <summary>
/// Valida históricos de prompts (arrays).
/// </summary>
public class HistoryPromptsValidator : AbstractValidator<PromptMessageVO[]>
{
    public HistoryPromptsValidator()
    {
        RuleFor(x => x.Length)
            .GreaterThan(0).WithMessage("O histórico de prompts não pode estar vazio.")
            .LessThanOrEqualTo(ChatCompletionValidatorsConstants.MaximumMessages)
            .WithMessage("O histórico de prompts não pode conter mais de 10 mensagens.");

        RuleForEach(x => x).SetValidator(new PromptMessageValidator());

        RuleFor(x => x)
            .Must(NotExceedMaxTokens)
            .WithMessage($"A soma total de tokens no histórico não pode exceder {ChatCompletionValidatorsConstants.MaxTotalTokens}.");
    }

    private static bool NotExceedMaxTokens(PromptMessageVO[] prompts)
    {
        if (prompts == null || prompts.Length == 0) return true;
        return prompts.Sum(p => p.TokenCount) <= ChatCompletionValidatorsConstants.MaxTotalTokens;
    }
}
#endif
