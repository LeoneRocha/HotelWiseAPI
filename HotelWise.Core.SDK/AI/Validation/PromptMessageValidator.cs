#if NET8_0_OR_GREATER
using FluentValidation;
using HotelWise.Core.SDK.AI.DTO;

namespace HotelWise.Core.SDK.AI.Validation;

/// <summary>
/// Valida mensagens individuais de prompt (<see cref="PromptMessageVO"/>).
/// </summary>
public class PromptMessageValidator : AbstractValidator<PromptMessageVO>
{
    private static readonly SmartCoreHub.Core.SDK.Service.AI.Validation.PromptMessageValidator SchValidator = new();

    /// <summary>
    /// Inicializa as regras de validação para <see cref="PromptMessageVO"/>.
    /// </summary>
    public PromptMessageValidator()
    {
        RuleFor(x => x).Custom((instance, context) =>
        {
            foreach (var error in SchValidator.Validate(instance).Errors)
                context.AddFailure(error);
        });
    }
}
#endif
