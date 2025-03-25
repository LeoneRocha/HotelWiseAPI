using FluentValidation;
using HotelWise.Domain.Model;
using HotelWise.Domain.Model.AI;

namespace HotelWise.Domain.Validator
{
    public class ChatSessionHistoryValidator : AbstractValidator<ChatSessionHistory>
    {
        public ChatSessionHistoryValidator()
        {
            // Validação para IdToken
            RuleFor(ch => ch.IdToken)
                .NotEmpty().WithMessage("O IdToken é obrigatório.")
                .MaximumLength(50).WithMessage("O IdToken deve ter no máximo 50 caracteres.")
                .Must(BeAValidGuid).WithMessage("O IdToken deve ser um GUID válido.");
             
            // Validação para PromptMessageHistory
            RuleFor(ch => ch.PromptMessageHistory)
                .NotNull().WithMessage("O histórico de mensagens é obrigatório.")
                .Must(h => h.Length > 0).WithMessage("O histórico de mensagens deve conter ao menos uma mensagem.");

            // Validação para TotalTokens
            RuleFor(ch => ch.CountMessages)
                .GreaterThanOrEqualTo(0).WithMessage("O total de messages deve ser maior ou igual a 0.");

            // Validação para CountMessages
            RuleFor(ch => ch.CountMessages)
                .GreaterThan(0).WithMessage("A contagem de mensagens deve ser maior que 0.");

            // Validação para SessionDateTime
            RuleFor(ch => ch.SessionDateTime)
                .NotEmpty().WithMessage("A data e hora da sessão são obrigatórias.")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("A data e hora da sessão não podem estar no futuro.");

            // Validação para IdUser (opcional)
            RuleFor(ch => ch.IdUser)
                .GreaterThan(0).When(ch => ch.IdUser.HasValue).WithMessage("O IdUser, se fornecido, deve ser maior que 0.");
        }

        private bool BeAValidGuid(string idToken)
        {
            return Guid.TryParse(idToken, out _); // Retorna true se for um GUID válido
        } 
    }
}
