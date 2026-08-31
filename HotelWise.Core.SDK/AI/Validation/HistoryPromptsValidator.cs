#if NET8_0_OR_GREATER
using FluentValidation;
using HotelWise.Core.SDK.AI.DTO;

namespace HotelWise.Core.SDK.AI.Validation;

/// <summary>
/// Valida históricos de prompts (arrays de <see cref="PromptMessageVO"/>).
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Service. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Service.AI.Validation.HistoryPromptsValidator. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class HistoryPromptsValidator : AbstractValidator<PromptMessageVO[]>
{
    private static readonly SmartCoreHub.Core.SDK.Service.AI.Validation.HistoryPromptsValidator SchValidator = new();

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
