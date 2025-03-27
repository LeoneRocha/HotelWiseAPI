using FluentValidation;
using HotelWise.Domain.Model.AI;

namespace HotelWise.Domain.Validator.AI
{
    public class ChatSessionHistoryValidator : AbstractValidator<ChatSessionHistory>
    {
        public ChatSessionHistoryValidator()
        {
            RuleFor(ch => ch.Title)
                .NotEmpty().WithMessage("O Title é obrigatório.")
                .MaximumLength(50).WithMessage("O Title deve ter no máximo 50 caracteres.");

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
            RuleFor(ch => ch.TotalTokensMessage)
                .GreaterThanOrEqualTo(0).WithMessage("O total de Tokens deve ser maior ou igual a 0.");

             
            RuleFor(ch => ch.CountMessages)
            .GreaterThanOrEqualTo(0).WithMessage("O total de messages deve ser maior ou igual a 0.");
              
            // Validação para CountMessages
            RuleFor(ch => ch.CountMessages)
                .GreaterThan(0).WithMessage("A contagem de mensagens deve ser maior que 0.");

            // Validação para SessionDateTime
            RuleFor(ch => ch.SessionDateTime)
                .NotEmpty().WithMessage("A data e hora da sessão são obrigatórias.")
                .GreaterThanOrEqualTo(DateTime.MinValue).WithMessage("A data e hora da sessão deve ser valida.");

            // Validação para IdUser (opcional)
            RuleFor(ch => ch.IdUser)
                .GreaterThan(0).When(ch => ch.IdUser.HasValue).WithMessage("O IdUser, se fornecido, deve ser maior que 0.");
        }

        private static bool BeAValidGuid(string idToken)
        {
            return Guid.TryParse(idToken, out _); // Retorna true se for um GUID válido
        }
    }
}
