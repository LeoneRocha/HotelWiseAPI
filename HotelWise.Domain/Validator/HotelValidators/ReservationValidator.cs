using FluentValidation;
using HotelWise.Domain.Enuns.Hotel;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Repository;
using HotelWise.Domain.Model.HotelModels;

namespace HotelWise.Domain.Validator.HotelValidators;

/// <summary>
/// Validador FluentValidation para <see cref="Reservation"/>, aplicando regras complexas de existência de quarto, disponibilidade no período, mínimo de diárias e antecedência para cancelamento.
/// </summary>
public class ReservationValidator : AbstractValidator<Reservation>
{
    private readonly IRoomRepository _roomRepository;
    private readonly IRoomAvailabilityRepository _roomAvailabilityRepository;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="ReservationValidator"/> com os repositórios necessários para validação assíncrona.
    /// </summary>
    /// <param name="roomRepository">Repositório de quartos.</param>
    /// <param name="roomAvailabilityRepository">Repositório de disponibilidades.</param>
    public ReservationValidator(IRoomRepository roomRepository, IRoomAvailabilityRepository roomAvailabilityRepository)
    {
        _roomRepository = roomRepository ?? throw new ArgumentNullException(nameof(roomRepository));
        _roomAvailabilityRepository = roomAvailabilityRepository ?? throw new ArgumentNullException(nameof(roomAvailabilityRepository));
        DefineRules();
    }

    /// <summary>
    /// Registra todas as regras de validação estruturais e de negócio para reservas.
    /// </summary>
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

        // Regra 6: validar antecedência mínima de 3 dias úteis para cancelamento
        RuleFor(r => r)
            .Must(CancellationHasValidBusinessDays)
            .When(r => r.Status == ReservationStatus.Cancelled)
            .WithMessage("A reserva só pode ser cancelada com pelo menos 3 dias úteis de antecedência.");
    }

    #region Regras Customizadas

    /// <summary>
    /// Valida assincronamente se o quarto informado existe no repositório.
    /// </summary>
    /// <param name="reservation">Entidade de reserva a validar.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns><c>true</c> se o quarto existir; caso contrário, <c>false</c>.</returns>
    private async Task<bool> RoomExistenceAsync(Reservation reservation, CancellationToken cancellationToken)
    {
        return await _roomRepository.ExistsAsync(r => r.Id == reservation.RoomId);
    }

    /// <summary>
    /// Verifica se a referência de navegação do quarto está preenchida.
    /// </summary>
    /// <param name="reservation">Entidade de reserva.</param>
    /// <returns><c>true</c> se o objeto Room estiver instanciado.</returns>
    private static bool ReservationHasRoomLoaded(Reservation reservation)
    {
        return reservation.Room is not null;
    }

    /// <summary>
    /// Verifica se o quarto associado possui status de disponibilidade operacional.
    /// </summary>
    /// <param name="reservation">Entidade de reserva.</param>
    /// <returns><c>true</c> se o quarto tiver status Available.</returns>
    private static bool ReservationHasAvailableRoomStatus(Reservation reservation)
    {
        return reservation.Room?.Status == RoomStatus.Available;
    }

    /// <summary>
    /// Verifica se o total de diárias da reserva respeita a quantidade mínima exigida pelo quarto.
    /// </summary>
    /// <param name="reservation">Entidade de reserva.</param>
    /// <returns><c>true</c> se atingir ou superar o mínimo de noites.</returns>
    private static bool ReservationMeetsMinimumNights(Reservation reservation)
    {
        var nights = (reservation.CheckOutDate.Date - reservation.CheckInDate.Date).Days;
        return nights >= reservation.Room.MinimumNights;
    }

    /// <summary>
    /// Valida assincronamente se todas as noites do período possuem estoque de vagas e moeda correspondente.
    /// </summary>
    /// <param name="reservation">Entidade de reserva.</param>
    /// <param name="cancellationToken">Token de cancelamento.</param>
    /// <returns><c>true</c> se houver disponibilidade suficiente para todas as diárias.</returns>
    private async Task<bool> AvailabilityIsSufficientAsync(Reservation reservation, CancellationToken cancellationToken)
    {
        var nights = (reservation.CheckOutDate.Date - reservation.CheckInDate.Date).Days;
        if (nights <= 0)
            return false;

        var availabilities = await _roomAvailabilityRepository.GetAvailabilityByDateRange(reservation.RoomId, reservation.CheckInDate.Date, reservation.CheckOutDate.Date);

        if (availabilities == null || availabilities.Length == 0)
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
    /// Verifica se uma data específica possui item de disponibilidade ativo e com quantidade maior que zero.
    /// </summary>
    /// <param name="availabilities">Coleção de disponibilidades cadastradas.</param>
    /// <param name="targetDate">Data da diária a ser verificada.</param>
    /// <param name="currency">Código da moeda desejada.</param>
    /// <returns><c>true</c> se a data estiver disponível.</returns>
    private static bool IsDateAvailable(RoomAvailability[] availabilities, DateTime targetDate, string currency)
    {
        return availabilities.Any(av => av.StartDate.Date <= targetDate &&
                                        av.EndDate.Date >= targetDate &&
                                        av.AvailabilityWithPrice.Any(item =>
                                            item.QuantityAvailable > 0 &&
                                            string.Equals(item.Currency, currency, StringComparison.OrdinalIgnoreCase)));
    }

    /// <summary>
    /// Valida se a solicitação de cancelamento foi feita com antecedência mínima de 3 dias úteis.
    /// </summary>
    /// <param name="reservation">Entidade de reserva a ser cancelada.</param>
    /// <returns><c>true</c> se a antecedência for de ao menos 3 dias úteis.</returns>
    private static bool CancellationHasValidBusinessDays(Reservation reservation)
    {
        var currentDate = DateTime.UtcNow.Date;
        var checkInDate = reservation.CheckInDate.Date;

        return CalculateBusinessDaysBetween(currentDate, checkInDate) >= 3;
    }

    /// <summary>
    /// Calcula a quantidade de dias úteis (segunda a sexta-feira) entre duas datas.
    /// </summary>
    /// <param name="start">Data inicial.</param>
    /// <param name="end">Data final.</param>
    /// <returns>Quantidade de dias úteis encontrados no intervalo.</returns>
    private static int CalculateBusinessDaysBetween(DateTime start, DateTime end)
    {
        if (end <= start)
            return 0;

        int totalDays = (end - start).Days;
        int businessDays = 0;

        for (int i = 0; i < totalDays; i++)
        {
            var day = start.AddDays(i).DayOfWeek;
            if (day != DayOfWeek.Saturday && day != DayOfWeek.Sunday)
                businessDays++;
        }

        return businessDays;
    }

    #endregion
}
