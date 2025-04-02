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

            DefineRules();
        }

        private void DefineRules()
        {
            // Validação para o ID do quarto
            RuleFor(ra => ra.RoomId)
                .GreaterThan(0)
                    .WithMessage("O ID do quarto é obrigatório e deve ser maior que 0.")
                .MustAsync(RoomExistsAsync)
                    .WithMessage("O ID do quarto fornecido não existe no sistema.");

            // Validação do período: datas não vazias, em ordem e sem estar no passado.
            RuleFor(ra => ra.StartDate)
                .NotEmpty()
                    .WithMessage("A data inicial é obrigatória.")
                .LessThan(ra => ra.EndDate)
                    .WithMessage("A data inicial deve ser anterior à data final.")
                .Must(date => date.Date >= DateTime.UtcNow.Date)
                    .WithMessage("A data inicial não pode ser no passado.");

            RuleFor(ra => ra.EndDate)
                .NotEmpty()
                    .WithMessage("A data final é obrigatória.")
                .GreaterThan(ra => ra.StartDate)
                    .WithMessage("A data final deve ser posterior à data inicial.");

            // Validação do array de disponibilidade
            RuleFor(ra => ra.AvailabilityWithPrice)
                .NotNull()
                    .WithMessage("A disponibilidade com preços é obrigatória.")
                .NotEmpty()
                    .WithMessage("É necessário pelo menos um período de disponibilidade.")
                .Must(HaveValidPrices)
                    .WithMessage("Todos os preços devem ser positivos.");

            // Validação para que as datas dos itens estejam dentro do período definido.
            RuleFor(ra => ra)
                .Must(HaveItemsWithinPeriod)
                    .WithMessage("Todas as datas dos itens devem estar entre a data inicial e a data final do período.");

            // Validação para que cada item tenha o DayOfWeek correto.
            RuleFor(ra => ra.AvailabilityWithPrice)
                .Must(HaveMatchingDayOfWeek)
                    .WithMessage("O dia da semana informado não corresponde à data do item de disponibilidade.");

            // Validação para evitar períodos conflitantes com registros já cadastrados.
            RuleFor(ra => ra)
                .MustAsync(NoPeriodOverlapAsync)
                    .WithMessage("O período de disponibilidade conflita com registros existentes para o mesmo quarto.");

            // Validação para evitar duplicidade de itens (mesma data e moeda) entre o novo registro e os já cadastrados.
            RuleFor(ra => ra)
                .MustAsync(ValidateNoDuplicateAvailabilityItemsAsync)
                    .WithMessage("Já existe um item cadastrado com a mesma data e moeda para este quarto.");
        }

        #region Métodos de Validação Auxiliar

        /// <summary>
        /// Verifica se o quarto existe no banco.
        /// </summary>
        private async Task<bool> RoomExistsAsync(long roomId, CancellationToken cancellationToken)
        {
            return await _roomRepository.ExistsAsync(r => r.Id == roomId);
        }

        /// <summary>
        /// Verifica se todos os preços dos itens são positivos.
        /// </summary>
        private static bool HaveValidPrices(RoomPriceAndAvailabilityItem[] items) => items.All(item => item.Price > 0);

        /// <summary>
        /// Garante que todas as datas dos itens estejam entre a StartDate e EndDate definidos na disponibilidade.
        /// </summary>
        private static bool HaveItemsWithinPeriod(RoomAvailability availability) => availability.AvailabilityWithPrice.All(item => item.Date.Date >= availability.StartDate.Date && item.Date.Date <= availability.EndDate.Date);

        /// <summary>
        /// Verifica que, para cada item, o DayOfWeek informado é igual ao DayOfWeek derivado da data.
        /// </summary>
        private static bool HaveMatchingDayOfWeek(RoomPriceAndAvailabilityItem[] items) => items.All(item => item.Date.DayOfWeek == item.DayOfWeek);

        /// <summary>
        /// Valida se o período (StartDate/EndDate) do registro não conflita com períodos já cadastrados para o mesmo quarto.
        /// </summary>
        private async Task<bool> NoPeriodOverlapAsync(RoomAvailability availability, CancellationToken cancellationToken)
        {
            var existingAvailabilities = await _roomAvailabilityRepository.GetAvailabilityByRoomId(availability.RoomId);
            // Há conflito de período se o novo registro se sobrepõe a um registro já existente
            return !existingAvailabilities.Any(existing => availability.StartDate < existing.EndDate && availability.EndDate > existing.StartDate);
        }

        /// <summary>
        /// Verifica que não há duplicatas de itens, considerando duplicidade interna (mesma data e moeda no registro atual)
        /// e duplicidade com registros existentes para o mesmo quarto.
        /// </summary>
        private async Task<bool> ValidateNoDuplicateAvailabilityItemsAsync(RoomAvailability availability, CancellationToken cancellationToken)
        {
            // Verifica duplicatas internas no registro atual.
            var hasInternalDuplicates = availability.AvailabilityWithPrice.GroupBy(item => new { item.Date, item.Currency }).Any(group => group.Count() > 1);
            if (hasInternalDuplicates)
                return false;

            // Verifica duplicidade em registros existentes.
            var existingAvailabilities = await _roomAvailabilityRepository.GetAvailabilityByRoomId(availability.RoomId);
            foreach (var newItem in availability.AvailabilityWithPrice)
            {
                if (existingAvailabilities.Any(existing => existing.AvailabilityWithPrice.Any(existingItem => existingItem.Date.Date == newItem.Date.Date && existingItem.Currency == newItem.Currency)))
                {
                    return false;
                }
            }
            return true;
        }

        #endregion
    }
}
