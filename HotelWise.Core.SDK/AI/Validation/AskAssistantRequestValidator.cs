#if NET8_0_OR_GREATER
using FluentValidation;
using HotelWise.Core.SDK.AI.Constants;
using HotelWise.Core.SDK.AI.DTO;

namespace HotelWise.Core.SDK.AI.Validation;

/// <summary>
/// Valida solicitações ao assistente conversacional (<see cref="AskAssistantRequest"/>).
/// Garante mensagem obrigatória e limites de comprimento para mensagem e token de sessão.
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Service. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Service.AI.Validation.AskAssistantRequestValidator. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
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
