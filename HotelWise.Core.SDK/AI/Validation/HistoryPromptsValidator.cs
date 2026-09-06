#if NET8_0_OR_GREATER
using FluentValidation;
using HotelWise.Core.SDK.AI.DTO;

using SmartCoreHub.Core.SDK.Common.Attributes;

namespace HotelWise.Core.SDK.AI.Validation;

/// <summary>
/// Valida históricos de prompts (arrays de <see cref="PromptMessageVO"/>).
/// </summary>
[SdkWrappedSource(targetType: "SmartCoreHub.Core.SDK.Service.AI.Validation.HistoryPromptsValidator", targetPackage: "SmartCoreHub.Core.SDK", description: "Casca/wrapper delegando para SmartCoreHub.Core.SDK.Service.AI.Validation.HistoryPromptsValidator em SmartCoreHub.Core.SDK.")]
public class HistoryPromptsValidator : AbstractValidator<PromptMessageVO[]>
{
    private static readonly SmartCoreHub.Core.SDK.Service.AI.Validation.HistoryPromptsValidator SchValidator = new();

    /// <summary>
    /// Inicializa as regras de validação para arrays de <see cref="PromptMessageVO"/>.
    /// </summary>
    public HistoryPromptsValidator()
    {
        RuleFor(x => x).Custom((instance, context) =>
        {
            foreach (var error in SchValidator.Validate(instance).Errors)
                context.AddFailure(error);
        });
    }
}
#endif
