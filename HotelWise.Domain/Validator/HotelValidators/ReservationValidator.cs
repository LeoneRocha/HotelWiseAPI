using FluentValidation;
using HotelWise.Domain.Enuns.Hotel;
using HotelWise.Domain.Interfaces.Entity;
using HotelWise.Domain.Model.HotelModels;

namespace HotelWise.Domain.Validator.HotelValidators
{
    public class ReservationValidator : AbstractValidator<Reservation>
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IRoomAvailabilityRepository _roomAvailabilityRepository;

        public ReservationValidator(IRoomRepository roomRepository, IRoomAvailabilityRepository roomAvailabilityRepository)
        {
            _roomRepository = roomRepository ?? throw new ArgumentNullException(nameof(roomRepository));
            _roomAvailabilityRepository = roomAvailabilityRepository ?? throw new ArgumentNullException(nameof(roomAvailabilityRepository));
            DefineRules();
        }

        private void DefineRules()
        {
            RuleFor(r => r.RoomId)
                .GreaterThan(0)
                .WithMessage("O RoomId é obrigatório e deve ser maior que 0.");

            RuleFor(r => r.CheckInDate)
                .NotEmpty()
                .WithMessage("A data de entrada é obrigatória.")
                .LessThan(r => r.CheckOutDate)
                .WithMessage("A data de entrada deve ser antes da data de saída.");

            RuleFor(r => r.CheckOutDate)
                .NotEmpty()
                .WithMessage("A data de saída é obrigatória.")
                .GreaterThan(r => r.CheckInDate)
                .WithMessage("A data de saída deve ser posterior à data de entrada.");

            RuleFor(r => r.ReservationDate)
                .NotEmpty()
                .WithMessage("A data da reserva é obrigatória.")
                .LessThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("A data da reserva não pode ser no futuro.");

            RuleFor(r => r.TotalAmount)
                .GreaterThan(0)
                .WithMessage("O valor total da reserva deve ser maior que 0.");

            RuleFor(r => r.Currency)
                .NotEmpty()
                .WithMessage("A moeda é obrigatória.")
                .Length(3)
                .WithMessage("A moeda deve ter exatamente 3 caracteres.");

            RuleFor(r => r.Status)
                .IsInEnum()
                .WithMessage("O status da reserva é inválido.");

            RuleFor(r => r)
                .MustAsync(RoomIsAvailableForPeriodAsync)
                .WithMessage("O quarto não está disponível ou não possui disponibilidade para o período e moeda selecionados.");
        }

        #region Validações de Disponibilidade e Quarto

        /// <summary>
        /// Combina as validações que dependem do estado do quarto e da disponibilidade para o período de reserva.
        /// </summary>
        private async Task<bool> RoomIsAvailableForPeriodAsync(Reservation reservation, CancellationToken cancellationToken)
        {
            if (!await ValidateRoomExistenceAsync(reservation, cancellationToken))
                return false;

            if (!ValidateRoomLoaded(reservation))
                return false;

            if (!ValidateRoomStatus(reservation))
                return false;

            return await ValidateAvailabilityForEachNightAsync(reservation, cancellationToken);
        }

        /// <summary>
        /// Verifica se o quarto existe no banco.
        /// </summary>
        private async Task<bool> ValidateRoomExistenceAsync(Reservation reservation, CancellationToken cancellationToken) => await _roomRepository.ExistsAsync(r => r.Id == reservation.RoomId);

        /// <summary>
        /// Verifica se o objeto Room foi carregado na reserva.
        /// </summary>
        private bool ValidateRoomLoaded(Reservation reservation) => reservation.Room is not null;

        /// <summary>
        /// Verifica se o quarto está com status Available.
        /// </summary>
        private bool ValidateRoomStatus(Reservation reservation) => reservation.Room?.Status == RoomStatus.Available;

        /// <summary>
        /// Para cada noite da reserva, valida se existe um item de disponibilidade com quantidade disponível e com a moeda compatível.
        /// </summary>
        private async Task<bool> ValidateAvailabilityForEachNightAsync(Reservation reservation, CancellationToken cancellationToken)
        {
            var nights = (reservation.CheckOutDate.Date - reservation.CheckInDate.Date).Days;
            if (nights <= 0)
                return false;

            // Obtém as disponibilidades dentro do intervalo de datas
            var availabilities = await _roomAvailabilityRepository.GetAvailabilityByDateRange(reservation.RoomId, reservation.CheckInDate.Date, reservation.CheckOutDate.Date);

            if (availabilities is null || availabilities.Length == 0)
                return false;

            for (var day = 0; day < nights; day++)
            {
                var targetDate = reservation.CheckInDate.Date.AddDays(day);
                if (!IsDateAvailable(availabilities, targetDate, reservation.Currency))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Verifica se, para a data alvo, existe pelo menos um item de disponibilidade com quantidade disponível e moeda compatível.
        /// </summary>
        private static bool IsDateAvailable(RoomAvailability[] availabilities, DateTime targetDate, string currency)
        {
            return availabilities.Any(av => av.StartDate.Date <= targetDate && av.EndDate.Date >= targetDate && av.AvailabilityWithPrice.Any(item => item.Date.Date == targetDate && item.QuantityAvailable > 0 && string.Equals(item.Currency, currency, StringComparison.OrdinalIgnoreCase)));
        }

        #endregion
    }
}
