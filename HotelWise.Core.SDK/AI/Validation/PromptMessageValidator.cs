#if NET8_0_OR_GREATER
using FluentValidation;
using HotelWise.Core.SDK.AI.Constants;
using HotelWise.Core.SDK.AI.DTO;
using HotelWise.Core.SDK.AI.Enums;
using HotelWise.Core.SDK.AI.Helpers;

namespace HotelWise.Core.SDK.AI.Validation;

/// <summary>
/// Valida mensagens individuais de prompt.
/// </summary>
public class PromptMessageValidator : AbstractValidator<PromptMessageVO>
{
    public PromptMessageValidator()
    {
        RuleFor(x => x.RoleType)
            .IsInEnum().WithMessage("O tipo de role é inválido.");

        RuleFor(x => x.TokenCount)
            .GreaterThanOrEqualTo(0).WithMessage("A contagem de tokens não pode ser negativa.");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("O conteúdo da mensagem é obrigatório.")
            .MaximumLength(ChatCompletionValidatorsConstants.MaxTextLength)
            .WithMessage($"A mensagem não pode exceder {ChatCompletionValidatorsConstants.MaxTextLength} caracteres.")
            .Must(BeWithinTokenLimit)
            .WithMessage($"A mensagem não pode exceder {ChatCompletionValidatorsConstants.MaxTokensPerMessage} tokens.")
            .When(x => x.RoleType != RoleAiPromptsType.Context && (x.DataContextRag == null || x.DataContextRag.Length == 0))
            .WithMessage("Quando DataContextRag estiver vazio, o conteúdo da mensagem deve ser preenchido corretamente.");

        RuleFor(x => x.Content)
            .Must(BeWithinTokenLimitContext)
            .WithMessage("O conteúdo para o contexto não pode exceder 100.000 tokens.")
            .When(x => x.RoleType == RoleAiPromptsType.Context);
    }

    private static bool BeWithinTokenLimitContext(string content) =>
        TokenCounterHelper.CountTokens(content) <= ChatCompletionValidatorsConstants.MaxTokensPerMessageContext;

    private static bool BeWithinTokenLimit(string content) =>
        TokenCounterHelper.CountTokens(content) <= ChatCompletionValidatorsConstants.MaxTokensPerMessage;
}
#endif
