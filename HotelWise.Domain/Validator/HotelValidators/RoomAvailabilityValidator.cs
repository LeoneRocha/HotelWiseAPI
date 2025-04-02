using FluentValidation;
using HotelWise.Domain.Model.HotelModels;

namespace HotelWise.Domain.Validator.HotelValidators
{
    public class RoomAvailabilityValidator : AbstractValidator<RoomAvailability>
    {
        public RoomAvailabilityValidator()
        {
            RuleFor(ra => ra.RoomId)
                .GreaterThan(0).WithMessage("O RoomId é obrigatório e deve ser maior que 0.");

            RuleFor(ra => ra.StartDate)
                .NotEmpty().WithMessage("A data inicial é obrigatória.")
                .LessThan(ra => ra.EndDate).WithMessage("A data inicial deve ser antes da data final.");

            RuleFor(ra => ra.EndDate)
                .NotEmpty().WithMessage("A data final é obrigatória.")
                .GreaterThan(ra => ra.StartDate).WithMessage("A data final deve ser depois da data inicial.");

            RuleFor(ra => ra.AvailabilityWithPrice)
                .NotNull().WithMessage("A disponibilidade com preços é obrigatória.")
                .Must(a => a.Length > 0).WithMessage("É necessário pelo menos um período de disponibilidade.");
        }
    }
}