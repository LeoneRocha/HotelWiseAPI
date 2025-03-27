using FluentValidation;
using HotelWise.Domain.Constants.IA;
using HotelWise.Domain.Dto;
using HotelWise.Domain.Enuns.IA;
using HotelWise.Domain.Helpers.AI;

namespace HotelWise.Domain.Validator.AI
{
    public class PromptMessageValidator : AbstractValidator<PromptMessageVO>
    {
        public PromptMessageValidator()
        {
            // Valida o tipo de role
            RuleFor(x => x.RoleType)
                .IsInEnum().WithMessage("O tipo de role é inválido.");

            // Valida se o TokenCount não é negativo
            RuleFor(x => x.TokenCount)
                .GreaterThanOrEqualTo(0).WithMessage("A contagem de tokens não pode ser negativa.");

            // Valida o Content somente se DataContextRag estiver vazio ou nulo
            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("O conteúdo da mensagem é obrigatório.")
                .MaximumLength(ChatCompletionValidatorsConstants.MaxTextLength).WithMessage($"A mensagem não pode exceder {ChatCompletionValidatorsConstants.MaxTextLength} caracteres.")
                .Must(BeWithinTokenLimit).WithMessage($"A mensagem não pode exceder {ChatCompletionValidatorsConstants.MaxTokensPerMessage} tokens.")
                .When(x => x.RoleType != RoleAiPromptsType.Context && (x.DataContextRag == null || x.DataContextRag.Length == 0))
                .WithMessage("Quando DataContextRag estiver vazio, o conteúdo da mensagem deve ser preenchido corretamente.");

            // Valida se o Content não excede 100.000 tokens quando RoleType é Context
            RuleFor(x => x.Content)
                .Must(BeWithinTokenLimitContext).WithMessage("O conteúdo para o contexto não pode exceder 100.000 tokens.")
                .When(x => x.RoleType == RoleAiPromptsType.Context);
        }

        private static bool BeWithinTokenLimitContext(string content)
        {
            // Verifica se o conteúdo está dentro do limite de tokens
            return TokenCounterHelper.CountTokens(content) <= ChatCompletionValidatorsConstants.MaxTokensPerMessageContext;
        }

        private static bool BeWithinTokenLimit(string content)
        {
            // Verifica se o conteúdo está dentro do limite de tokens
            return TokenCounterHelper.CountTokens(content) <= ChatCompletionValidatorsConstants.MaxTokensPerMessage;
        }
    }
}
