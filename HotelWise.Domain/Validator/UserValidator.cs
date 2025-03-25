using FluentValidation;
using global::HotelWise.Domain.Model;


namespace HotelWise.Domain.Validator
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            // Validação para o ID
            RuleFor(u => u.Id)
                .GreaterThan(0).WithMessage("O ID deve ser um número positivo.");

            // Validação para o Nome
            RuleFor(u => u.Name)
                .NotEmpty().WithMessage("O nome é obrigatório.")
                .MaximumLength(255).WithMessage("O nome deve ter no máximo 255 caracteres.");

            // Validação para o Email
            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("O email é obrigatório.")
                .MaximumLength(100).WithMessage("O email deve ter no máximo 100 caracteres.")
                .EmailAddress().WithMessage("O email fornecido é inválido.");

            // Validação para o Login
            RuleFor(u => u.Login)
                .NotEmpty().WithMessage("O login é obrigatório.")
                .MaximumLength(25).WithMessage("O login deve ter no máximo 25 caracteres.");

            // Validação para a Senha
            RuleFor(u => u.PasswordHash)
                .NotNull().WithMessage("O hash da senha é obrigatório.");

            RuleFor(u => u.PasswordSalt)
                .NotNull().WithMessage("O salt da senha é obrigatório.");

            // Validação para o Papel (Role)
            RuleFor(u => u.Role)
                .NotEmpty().WithMessage("O papel é obrigatório.")
                .MaximumLength(50).WithMessage("O papel deve ter no máximo 50 caracteres.");

            // Validação para o Admin
            RuleFor(u => u.Admin)
                .NotNull().WithMessage("A informação de admin é obrigatória.");

            // Validação para o Idioma (Language)
            RuleFor(u => u.Language)
                .MaximumLength(10).WithMessage("O idioma deve ter no máximo 10 caracteres.");

            // Validação para o Fuso Horário (TimeZone)
            RuleFor(u => u.TimeZone)
                .MaximumLength(255).WithMessage("O fuso horário deve ter no máximo 255 caracteres.");

            // Validação para o RefreshToken
            RuleFor(u => u.RefreshToken)
                .NotNull().WithMessage("O refresh token é obrigatório.");

            // Validação para o RefreshTokenExpiryTime
            RuleFor(u => u.RefreshTokenExpiryTime)
                .NotEmpty().WithMessage("O tempo de expiração do refresh token é obrigatório.");
        }
    }
}
