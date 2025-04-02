using FluentValidation;
using HotelWise.Domain.Interfaces.Entity;
using HotelWise.Domain.Model.HotelModels;

namespace HotelWise.Domain.Validator.HotelValidators
{
    public class RoomAvailabilityValidator : AbstractValidator<RoomAvailability>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IRoomAvailabilityRepository _roomAvailabilityRepository;

        public RoomAvailabilityValidator(
            IRoomRepository roomRepository,
            IRoomAvailabilityRepository roomAvailabilityRepository)
        {
            _roomRepository = roomRepository ?? throw new ArgumentNullException(nameof(roomRepository));
            _roomAvailabilityRepository = roomAvailabilityRepository ?? throw new ArgumentNullException(nameof(roomAvailabilityRepository));

            // Validação para RoomId
            RuleFor(ra => ra.RoomId)
                .GreaterThan(0).WithMessage("O ID do quarto é obrigatório e deve ser maior que 0.")
                .MustAsync(RoomExistsAsync).WithMessage("O ID do quarto fornecido não existe no sistema.");

            // Validação para StartDate e EndDate
            RuleFor(ra => ra.StartDate)
                .NotEmpty().WithMessage("A data inicial é obrigatória.")
                .LessThan(ra => ra.EndDate).WithMessage("A data inicial deve ser anterior à data final.");

            RuleFor(ra => ra.EndDate)
                .NotEmpty().WithMessage("A data final é obrigatória.")
                .GreaterThan(ra => ra.StartDate).WithMessage("A data final deve ser posterior à data inicial.");

            // Validação para Preços e Disponibilidade
            RuleFor(ra => ra.AvailabilityWithPrice)
                .NotNull().WithMessage("A disponibilidade com preços é obrigatória.")
                .Must(a => a.Length > 0).WithMessage("É necessário pelo menos um período de disponibilidade.")
                .Must(ValidatePriceConsistency).WithMessage("Os preços devem ser positivos e consistentes.")
                .Must(ValidateCurrencyConsistency).WithMessage("Todas as moedas devem ser iguais.");

            // Validação de sobreposição de períodos
            RuleFor(ra => ra)
                .MustAsync(NoDateOverlapsAsync).WithMessage("Os períodos de disponibilidade não podem se sobrepor.");
        }

        /// <summary>
        /// Verifica se o quarto existe no sistema.
        /// </summary>
        private async Task<bool> RoomExistsAsync(long roomId, CancellationToken cancellationToken)
        {
            return await _roomRepository.ExistsAsync(r => r.Id == roomId);
        }

        /// <summary>
        /// Garante que os preços sejam positivos e consistentes.
        /// </summary>
        private bool ValidatePriceConsistency(RoomPriceAndAvailabilityItem[] availabilityItems)
        {
            return availabilityItems.All(item => item.Price > 0);
        }

        /// <summary>
        /// Garante que todas as moedas nos itens de disponibilidade sejam iguais.
        /// </summary>
        private bool ValidateCurrencyConsistency(RoomPriceAndAvailabilityItem[] availabilityItems)
        {
            var currency = availabilityItems.First().Currency;
            return availabilityItems.All(item => item.Currency == currency);
        }

        /// <summary>
        /// Garante que não existem períodos sobrepostos para o mesmo quarto.
        /// </summary>
        private async Task<bool> NoDateOverlapsAsync(RoomAvailability availability, CancellationToken cancellationToken)
        {
            var existingAvailabilities = await _roomAvailabilityRepository.GetAvailabilityByRoomId(availability.RoomId);
            return !existingAvailabilities.Any(existing =>
                availability.StartDate < existing.EndDate && availability.EndDate > existing.StartDate);
        }
    }
}
