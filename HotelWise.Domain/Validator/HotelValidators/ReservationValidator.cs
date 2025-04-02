using FluentValidation;
using HotelWise.Domain.Model.HotelModels;

namespace HotelWise.Domain.Validator.HotelValidators
{
    public class ReservationValidator : AbstractValidator<Reservation>
    {
        public ReservationValidator()
        {
            RuleFor(r => r.RoomId)
                .GreaterThan(0).WithMessage("O RoomId é obrigatório e deve ser maior que 0.");

            RuleFor(r => r.CheckInDate)
                .NotEmpty().WithMessage("A data de entrada é obrigatória.")
                .LessThan(r => r.CheckOutDate).WithMessage("A data de entrada deve ser antes da data de saída.");

            RuleFor(r => r.CheckOutDate)
                .NotEmpty().WithMessage("A data de saída é obrigatória.")
                .GreaterThan(r => r.CheckInDate).WithMessage("A data de saída deve ser depois da data de entrada.");

            RuleFor(r => r.ReservationDate)
                .NotEmpty().WithMessage("A data da reserva é obrigatória.")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("A data da reserva não pode ser no futuro.");

            RuleFor(r => r.TotalAmount)
                .GreaterThan(0).WithMessage("O valor total da reserva deve ser maior que 0.");

            RuleFor(r => r.Currency)
                .NotEmpty().WithMessage("A moeda é obrigatória.")
                .Length(3).WithMessage("A moeda deve ter exatamente 3 caracteres.");

            RuleFor(r => r.Status)
                .IsInEnum().WithMessage("O status da reserva é inválido.");
        }
    } 
} 