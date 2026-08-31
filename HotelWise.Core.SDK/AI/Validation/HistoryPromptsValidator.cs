#if NET8_0_OR_GREATER
using FluentValidation;
using HotelWise.Core.SDK.AI.Constants;
using HotelWise.Core.SDK.AI.DTO;

namespace HotelWise.Core.SDK.AI.Validation;

/// <summary>
/// Valida históricos de prompts (arrays de <see cref="PromptMessageVO"/>).
/// Garante quantidade mínima/máxima de mensagens, validação individual via
/// <see cref="PromptMessageValidator"/> e limite total de tokens para chat completion.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Service. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Service.AI.Validation.HistoryPromptsValidator. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class HistoryPromptsValidator : AbstractValidator<PromptMessageVO[]>
{
    /// <summary>
    /// Inicializa as regras de validação do histórico de prompts.
    /// </summary>
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

    /// <summary>
    /// Verifica se a soma de tokens do histórico não excede o limite configurado.
    /// </summary>
    /// <param name="prompts">Histórico de prompts.</param>
    /// <returns><c>true</c> se dentro do limite; caso contrário, <c>false</c>.</returns>
    private static bool NotExceedMaxTokens(PromptMessageVO[] prompts)
    {
        if (prompts == null || prompts.Length == 0) return true;
        return prompts.Sum(p => p.TokenCount) <= ChatCompletionValidatorsConstants.MaxTotalTokens;
    }
}
#endif
