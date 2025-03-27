using FluentValidation;
using HotelWise.Domain.Constants.IA;
using HotelWise.Domain.Dto;

namespace HotelWise.Domain.Validator.AI
{
    public class HistoryPromptsValidator : AbstractValidator<PromptMessageVO[]>
    {
        public HistoryPromptsValidator()
        {
            // Valida o tamanho máximo do array de prompts (por exemplo: máximo de 10 prompts)
            RuleFor(x => x.Length)
                .GreaterThan(0).WithMessage("O histórico de prompts não pode estar vazio.")
                .LessThanOrEqualTo(ChatCompletionValidatorsConstants.MaximumMessages).WithMessage("O histórico de prompts não pode conter mais de 10 mensagens.");

            // Valida cada elemento no array
            RuleForEach(x => x).SetValidator(new PromptMessageValidator());

            // Valida a soma total dos tokens no array
            RuleFor(x => x)
                .Must(NotExceedMaxTokens).WithMessage($"A soma total de tokens no histórico não pode exceder {ChatCompletionValidatorsConstants.MaxTotalTokens}.");
        }

        /// <summary>
        /// Valida se a soma total dos tokens no array não excede o limite máximo.
        /// </summary>
        /// <param name="prompts">Array de PromptMessageVO.</param>
        /// <returns>True se a soma total de tokens estiver dentro do limite, caso contrário false.</returns>
        private static bool NotExceedMaxTokens(PromptMessageVO[] prompts)
        {
            if (prompts == null || prompts.Length == 0) return true; // Nenhuma mensagem, válido
            return prompts.Sum(p => p.TokenCount) <= ChatCompletionValidatorsConstants.MaxTotalTokens;
        }
    }


}
