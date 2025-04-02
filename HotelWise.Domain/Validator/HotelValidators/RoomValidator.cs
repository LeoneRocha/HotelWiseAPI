using FluentValidation;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces;
using HotelWise.Domain.Model.HotelModels;

namespace HotelWise.Domain.Validator.HotelValidators
{
    public class RoomValidator : AbstractValidator<Room>
    { 
        private readonly IHotelRepository _hotelRepository;

        public RoomValidator(IRoomRepository roomRepository, IHotelRepository hotelRepository)
        { 
            _hotelRepository = hotelRepository ?? throw new ArgumentNullException(nameof(hotelRepository));

            // Validação para HotelId
            RuleFor(r => r.HotelId)
                .GreaterThan(0).WithMessage("O ID do hotel é obrigatório e deve ser maior que 0.")
                .MustAsync(HotelExistsAsync).WithMessage("O ID do hotel fornecido não existe no sistema.");

            // Validação para RoomType (Enum)
            RuleFor(r => r.RoomType)
                .IsInEnum().WithMessage("O tipo de quarto fornecido é inválido.");

            // Validação para Capacity
            RuleFor(r => (int)r.Capacity)
                .GreaterThan(0).WithMessage("A capacidade do quarto deve ser maior que 0.")
                .LessThanOrEqualTo(100).WithMessage("A capacidade do quarto não pode exceder 100 pessoas.");

            // Validação para Description
            RuleFor(r => r.Description)
                .NotEmpty().WithMessage("A descrição do quarto é obrigatória.")
                .MaximumLength(1000).WithMessage("A descrição do quarto não pode ultrapassar 1000 caracteres.");

            // Validação para Status (Enum)
            RuleFor(r => r.Status)
                .IsInEnum().WithMessage("O status do quarto é inválido.");

            // Validação para MinimumNights
            RuleFor(r => (int)r.MinimumNights)
                .GreaterThanOrEqualTo(1).WithMessage("O número mínimo de noites deve ser pelo menos 1.")
                .LessThanOrEqualTo(30).WithMessage("O número mínimo de noites não pode exceder 30.");

            // Validação para CreatedDate
            RuleFor(r => r.CreatedDate)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("A data de criação do registro não pode ser no futuro.");

            // Validação para ModifyDate
            RuleFor(r => r.ModifyDate)
                .GreaterThan(r => r.CreatedDate).WithMessage("A data de modificação deve ser maior que a data de criação."); 
        }

        /// <summary>
        /// Método para verificar se o HotelId existe no sistema.
        /// </summary>
        private async Task<bool> HotelExistsAsync(long hotelId, CancellationToken cancellationToken)
        {
            return await _hotelRepository.ExistsAsync(h => h.HotelId == hotelId);
        } 
    }
}
