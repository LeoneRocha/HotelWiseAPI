#if NET8_0_OR_GREATER
using FluentValidation;
using HotelWise.Core.SDK.AI.Constants;
using HotelWise.Core.SDK.AI.DTO;
using HotelWise.Core.SDK.AI.Enums;
using HotelWise.Core.SDK.AI.Helpers;

namespace HotelWise.Core.SDK.AI.Validation;

/// <summary>
/// Valida mensagens individuais de prompt (<see cref="PromptMessageVO"/>).
/// Aplica regras de papel, conteúdo, comprimento e limites de tokens,
/// com tratamento especial para mensagens de contexto RAG.
/// </summary>
public class PromptMessageValidator : AbstractValidator<PromptMessageVO>
{
    /// <summary>
    /// Inicializa as regras de validação de mensagem de prompt.
    /// </summary>
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

    /// <summary>
    /// Verifica se o conteúdo de contexto está dentro do limite de tokens RAG.
    /// </summary>
    /// <param name="content">Conteúdo da mensagem.</param>
    /// <returns><c>true</c> se dentro do limite.</returns>
    private static bool BeWithinTokenLimitContext(string content) =>
        TokenCounterHelper.CountTokens(content) <= ChatCompletionValidatorsConstants.MaxTokensPerMessageContext;

    /// <summary>
    /// Verifica se o conteúdo comum está dentro do limite de tokens por mensagem.
    /// </summary>
    /// <param name="content">Conteúdo da mensagem.</param>
    /// <returns><c>true</c> se dentro do limite.</returns>
    private static bool BeWithinTokenLimit(string content) =>
        TokenCounterHelper.CountTokens(content) <= ChatCompletionValidatorsConstants.MaxTokensPerMessage;
}
#endif
