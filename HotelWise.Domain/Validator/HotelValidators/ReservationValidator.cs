using FluentValidation;
using HotelWise.Domain.Enuns.Hotel;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Repository;
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
            // Validações básicas dos campos
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

            // Regra 1: verificar se o quarto existe
            RuleFor(r => r)
                .MustAsync(RoomExistenceAsync)
                .WithMessage("O quarto informado não existe.");

            // Regra 2: verificar se o objeto Room está carregado
            RuleFor(r => r)
                .Must(ReservationHasRoomLoaded)
                .WithMessage("O objeto Room não foi carregado na reserva.");

            // Regra 3: verificar se o quarto está disponível (status == Available)
            RuleFor(r => r)
                .Must(ReservationHasAvailableRoomStatus)
                .WithMessage("O quarto não está disponível para reserva.");

            // Regra 4: verificar se o número de noites da reserva atende ao mínimo exigido
            RuleFor(r => r)
                .Must(ReservationMeetsMinimumNights)
                .WithMessage(r =>
                {
                    var min = r.Room?.MinimumNights ?? 0;
                    return $"O período da reserva deve ter, no mínimo, {min} noites.";
                });

            // Regra 5: verificar se há disponibilidade para cada noite com a moeda selecionada
            RuleFor(r => r)
                .MustAsync(AvailabilityIsSufficientAsync)
                .WithMessage("Não há disponibilidade para todas as noites, com a moeda selecionada, no período informado.");
        }

        #region Regras Customizadas

        private async Task<bool> RoomExistenceAsync(Reservation reservation, CancellationToken cancellationToken)
        {
            // Verifica se o quarto existe no banco.
            return await _roomRepository.ExistsAsync(r => r.Id == reservation.RoomId);
        }

        private static bool ReservationHasRoomLoaded(Reservation reservation)
        {
            // Verifica se o objeto Room foi carregado na entidade de reserva.
            return reservation.Room is not null;
        }

        private static bool ReservationHasAvailableRoomStatus(Reservation reservation)
        {
            // Verifica se o objeto Room tem status Available.
            return reservation.Room?.Status == RoomStatus.Available;
        }

        private static bool ReservationMeetsMinimumNights(Reservation reservation)
        {
            // Calcula o número de noites da reserva.
            var nights = (reservation.CheckOutDate.Date - reservation.CheckInDate.Date).Days;
            // Garante que o número de noites seja maior ou igual ao mínimo exigido pelo quarto.
            return nights >= reservation.Room.MinimumNights;
        }

        private async Task<bool> AvailabilityIsSufficientAsync(Reservation reservation, CancellationToken cancellationToken)
        {
            var nights = (reservation.CheckOutDate.Date - reservation.CheckInDate.Date).Days;
            if (nights <= 0)
                return false;

            // Obtém as disponibilidades para o período informado.
            var availabilities = await _roomAvailabilityRepository.GetAvailabilityByDateRange(reservation.RoomId, reservation.CheckInDate.Date, reservation.CheckOutDate.Date);

            if (availabilities == null || availabilities.Length == 0)
                return false;

            // Para cada noite, verifica se há um item de disponibilidade com quantidade disponível e moeda compatível.
            for (var day = 0; day < nights; day++)
            {
                var targetDate = reservation.CheckInDate.Date.AddDays(day);
                if (!IsDateAvailable(availabilities, targetDate, reservation.Currency))
                    return false;
            }
            return true;
        }

        private static bool IsDateAvailable(RoomAvailability[] availabilities, DateTime targetDate, string currency)
        {
            return availabilities.Any(av => av.StartDate.Date <= targetDate && av.EndDate.Date >= targetDate && av.AvailabilityWithPrice.Any(item => item.Date.Date == targetDate && item.QuantityAvailable > 0 && string.Equals(item.Currency, currency, StringComparison.OrdinalIgnoreCase)));
        }

        #endregion
    }
}
