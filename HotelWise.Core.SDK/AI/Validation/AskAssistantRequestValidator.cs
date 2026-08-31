#if NET8_0_OR_GREATER
using FluentValidation;
using HotelWise.Core.SDK.AI.DTO;

namespace HotelWise.Core.SDK.AI.Validation;

/// <summary>
/// Valida solicitações ao assistente conversacional (<see cref="AskAssistantRequest"/>).
/// </summary>
[Obsolete("Depreciado. Migrado para SmartCoreHub.Core.SDK na camada Service. Use o pacote NuGet SmartCoreHub.Core.SDK — tipo SmartCoreHub.Core.SDK.Service.AI.Validation.AskAssistantRequestValidator. Após publicar o NuGet, HotelWise.Core.SDK será só casca (PackageReference + wrappers) e delegará a SmartCoreHub.Core.SDK.")]
public class AskAssistantRequestValidator : AbstractValidator<AskAssistantRequest>
{
    private static readonly SmartCoreHub.Core.SDK.Service.AI.Validation.AskAssistantRequestValidator SchValidator = new();

    public AskAssistantRequestValidator()
    {
        RuleFor(x => x).Custom((instance, context) =>
        {
            foreach (var error in SchValidator.Validate(instance).Errors)
                context.AddFailure(error);
        });
    }
}
#endif
