using FluentValidation;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Repository;
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

            // Validação do período
            RuleFor(ra => ra.StartDate)
                .NotEmpty()
                    .WithMessage("A data inicial é obrigatória.")
                .LessThan(ra => ra.EndDate)
                    .WithMessage("A data inicial deve ser anterior à data final.")
                .Must(date => date.Date >= DateTime.UtcNow.Date)
                .When(x => x.Id == 0)
                    .WithMessage("A data inicial não pode ser no passado.");

            RuleFor(ra => ra.EndDate)
                .NotEmpty()
                    .WithMessage("A data final é obrigatória.")
                .GreaterThan(ra => ra.StartDate)
                    .WithMessage("A data final deve ser posterior à data inicial.");

            // Validação para Currency
            RuleFor(ra => ra.Currency)
                .NotEmpty()
                    .WithMessage("A moeda é obrigatória.")
                .Length(3)
                    .WithMessage("A moeda deve ter exatamente 3 caracteres.")
                .Must(BeValidCurrency)
                    .WithMessage("A moeda fornecida não é válida.");

            // Validação para evitar Currency de itens
            RuleFor(ra => ra)
                .MustAsync(ValidateCurrencyAvailabilityItemsAsync)
                .WithMessage("A moeda deve ser igual para os precos do periodo.");


            // Validação do array de disponibilidade
            RuleFor(ra => ra.AvailabilityWithPrice)
                .NotNull()
                    .WithMessage("A disponibilidade com preços é obrigatória.")
                .NotEmpty()
                    .WithMessage("É necessário pelo menos um período de disponibilidade.")
                .Must(HaveValidPrices)
                    .WithMessage("Todos os preços devem ser positivos.");

            // Validação de conflitos no período
            RuleFor(ra => ra)
                .MustAsync(NoPeriodOverlapAsync)
                .When(x => x.Id == 0)
                .WithMessage("O período de disponibilidade conflita com registros existentes para o mesmo quarto.");

            // Validação para evitar duplicidade de itens
            RuleFor(ra => ra)
                .MustAsync(ValidateNoDuplicateAvailabilityItemsAsync)
                .When(x => x.Id == 0)
                .WithMessage("Já existe um item cadastrado com a mesma data e moeda para este quarto.");

            RuleFor(ra => ra)
              .MustAsync((ra, cancellationToken) => PeriodCannotBeModifiedAsync(ra, cancellationToken))
              .When(ra => ra.Id > 0) // Aplicado apenas em casos de edição
              .WithMessage("O período não pode ser alterado ao editar uma disponibilidade existente.");
        }

        #region Métodos de Validação Auxiliar
        /// <summary>
        /// Verifica se o período não foi modificado em uma edição.
        /// </summary>
        private async Task<bool> PeriodCannotBeModifiedAsync(RoomAvailability availability, CancellationToken cancellationToken)
        {
            var existingAvailability = await _roomAvailabilityRepository.GetByIdAsync(availability.Id);

            if (existingAvailability == null)
                return false; // Caso não encontre o registro existente, considere inválido.

            // Compara os períodos do registro existente com os valores novos
            return existingAvailability.StartDate == availability.StartDate &&
                   existingAvailability.EndDate == availability.EndDate;
        }
        /// <summary>
        /// Valida se todos os itens de disponibilidade têm a mesma moeda que a definida na disponibilidade principal.
        /// </summary>
        private static async Task<bool> ValidateCurrencyAvailabilityItemsAsync(RoomAvailability availability, CancellationToken cancellationToken)
        {
            if (availability.AvailabilityWithPrice == null || availability.AvailabilityWithPrice.Length == 0)
            {
                return true; // Se não há itens, consideramos válido.
            }
            // Verifica se todos os itens têm a mesma moeda que a definida na propriedade Currency da disponibilidade.
            return availability.AvailabilityWithPrice.All(item => item.Currency == availability.Currency);
        }

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
        private static bool HaveValidPrices(RoomPriceAndAvailabilityItem[] items) =>
            items.All(item => item.Price > 0);

        /// <summary>
        /// Valida se o código de moeda é válido (ISO 4217).
        /// </summary>
        private static bool BeValidCurrency(string currency)
        {
            // Liste as moedas válidas conforme necessidade.
            var validCurrencies = new[] { "USD", "BRL", "EUR", "JPY" }; // Exemplos
            return validCurrencies.Contains(currency);
        }

        /// <summary>
        /// Valida se o período (StartDate/EndDate) do registro não conflita com períodos já cadastrados.
        /// </summary>
        private async Task<bool> NoPeriodOverlapAsync(RoomAvailability availability, CancellationToken cancellationToken)
        {
            var existingAvailabilities = await _roomAvailabilityRepository.GetAvailabilityByRoomId(availability.RoomId);
            return !existingAvailabilities.Any(existing =>
                availability.StartDate < existing.EndDate && availability.EndDate > existing.StartDate);
        }

        /// <summary>
        /// Verifica que não há duplicatas de itens entre registros.
        /// </summary>
        private async Task<bool> ValidateNoDuplicateAvailabilityItemsAsync(RoomAvailability availability, CancellationToken cancellationToken)
        {
            var hasInternalDuplicates = availability.AvailabilityWithPrice.GroupBy(item => new { item.DayOfWeek, item.Currency }).Any(group => group.Count() > 1);

            if (hasInternalDuplicates)
                return false;

            var existingAvailabilities = await _roomAvailabilityRepository.GetAvailabilityByRoomId(availability.RoomId);
            return !availability.AvailabilityWithPrice.Any(newItem =>
                existingAvailabilities.SelectMany(existing => existing.AvailabilityWithPrice).Any(existingItem =>
                    existingItem.DayOfWeek == newItem.DayOfWeek && existingItem.Currency == newItem.Currency));
        }
        #endregion
    }
}