#if NET8_0_OR_GREATER
using FluentValidation;
using HotelWise.Core.SDK.AI.DTO;

namespace HotelWise.Core.SDK.AI.Validation;

/// <summary>
/// Valida solicitações ao assistente conversacional (<see cref="AskAssistantRequest"/>).
/// </summary>
public class AskAssistantRequestValidator : AbstractValidator<AskAssistantRequest>
{
    private static readonly SmartCoreHub.Core.SDK.Service.AI.Validation.AskAssistantRequestValidator SchValidator = new();

    /// <summary>
    /// Inicializa as regras de validação para <see cref="AskAssistantRequest"/>.
    /// </summary>
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
