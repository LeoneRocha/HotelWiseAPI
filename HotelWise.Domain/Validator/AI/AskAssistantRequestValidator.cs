using FluentValidation;
using HotelWise.Domain.Constants.IA;
using HotelWise.Domain.Dto;
namespace HotelWise.Domain.Validator.AI
{
    public class AskAssistantRequestValidator : AbstractValidator<AskAssistantRequest>
    {
        public AskAssistantRequestValidator()
        {
            // Valida se a mensagem não está vazia
            RuleFor(x => x.Message)
                .NotEmpty().WithMessage("A mensagem é obrigatória.")
                .MaximumLength(ChatCompletionValidatorsConstants.MaxTextLength).WithMessage($"A mensagem não pode exceder {ChatCompletionValidatorsConstants.MaxTextLength} caracteres.");

            // Valida o token se ele for fornecido
            RuleFor(x => x.Token)
                .MaximumLength(ChatCompletionValidatorsConstants.MaxTextToken).WithMessage($"O token não pode exceder {ChatCompletionValidatorsConstants.MaxTextToken} caracteres.");
        }
    }
}