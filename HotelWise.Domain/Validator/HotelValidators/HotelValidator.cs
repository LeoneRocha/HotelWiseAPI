using FluentValidation;
using HotelWise.Domain.Model.HotelModels;

namespace HotelWise.Domain.Validator.HotelValidators;

/// <summary>
/// Validador FluentValidation para a entidade <see cref="Hotel"/>, garantindo conformidade de nomes, preços, estrelas e dados de localização.
/// </summary>
public class HotelValidator : AbstractValidator<Hotel>
{
    /// <summary>
    /// Configura as regras de validação para os dados cadastrais do hotel.
    /// </summary>
    public HotelValidator()
    {
        RuleFor(h => h.HotelName).NotEmpty().MaximumLength(100);
        RuleFor(h => h.Description).MaximumLength(1000);
        RuleFor(h => (int)h.Stars).InclusiveBetween(1, 5);
        RuleFor(h => h.InitialRoomPrice).GreaterThan(0);
        RuleFor(h => h.ZipCode).MaximumLength(10);
        RuleFor(h => h.StateCode).MaximumLength(2);
        RuleFor(h => h.Location).MaximumLength(200);
        RuleFor(h => h.City).MaximumLength(200);
    }
}
