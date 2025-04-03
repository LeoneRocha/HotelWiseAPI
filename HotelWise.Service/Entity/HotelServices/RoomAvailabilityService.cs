using AutoMapper;
using FluentValidation;
using HotelWise.Domain.Dto;
using HotelWise.Domain.Dto.Enitty.HotelDtos;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Repository;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Service;
using HotelWise.Domain.Model.HotelModels;
using HotelWise.Service.Generic;
using Serilog;

namespace HotelWise.Service.Entity
{
    public class RoomAvailabilityService : GenericEntityServiceBase<RoomAvailability, RoomAvailabilityDto>, IRoomAvailabilityService
    {
        private readonly IRoomAvailabilityRepository _roomAvailabilityRepository;
        private readonly IRoomRepository _roomRepository;

        public RoomAvailabilityService(ILogger logger,
            IRoomAvailabilityRepository repository,
            IRoomRepository roomRepository,
            IMapper mapper,
            IValidator<RoomAvailability> entityValidator
            ) : base(repository, mapper, logger, entityValidator)
        {
            _roomAvailabilityRepository = repository;
            _roomRepository = roomRepository;
        }
        /// <summary>
        /// Cria uma nova disponibilidade de quarto após validação de negócios.
        /// </summary>
        public override async Task<ServiceResponse<RoomAvailabilityDto>> CreateAsync(RoomAvailabilityDto availabilityDto)
        {
            var response = new ServiceResponse<RoomAvailabilityDto>();

            // Mapeia o DTO para a entidade
            var roomAvailability = _mapper.Map<RoomAvailability>(availabilityDto);

            // Valida a disponibilidade antes de criar
            var validationResult = await _entityValidator.ValidateAsync(roomAvailability);
            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return response;
            }

            // Insere a disponibilidade no banco
            var createdAvailability = await _repository.AddAsync(roomAvailability);

            // Retorna a disponibilidade criada no formato DTO
            response.Data = _mapper.Map<RoomAvailabilityDto>(createdAvailability);
            response.Success = true;
            response.Message = "Disponibilidade criada com sucesso.";
            return response;
        }

        /// <summary>
        /// Cria múltiplas disponibilidades em lote.
        /// </summary>
        /// <param name="availabilitiesDto">Lista de disponibilidades</param>
        public async Task<ServiceResponse<string>> CreateBatchAsync(RoomAvailabilityDto[] availabilitiesDto)
        {
            var response = new ServiceResponse<string>();

            // Mapeia os DTOs para entidades
            var roomAvailabilities = _mapper.Map<RoomAvailability[]>(availabilitiesDto);

            // Valida cada disponibilidade antes de adicionar
            foreach (var availability in roomAvailabilities)
            {
                var validationResult = await _entityValidator.ValidateAsync(availability);
                if (!validationResult.IsValid)
                {
                    response.Success = false;
                    response.Message = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                    return response;
                }
            }

            // Insere no banco em lote
            await _repository.AddRangeAsync(roomAvailabilities);

            response.Success = true;
            response.Message = "Disponibilidades criadas em lote com sucesso.";
            return response;
        }
         
        /// <summary>
        /// Atualiza uma disponibilidade de quarto existente.
        /// </summary>
        public override async Task<ServiceResponse<RoomAvailabilityDto>> UpdateAsync(RoomAvailabilityDto availabilityDto)
        {
            var response = new ServiceResponse<RoomAvailabilityDto>();
            var availabilityId = availabilityDto.Id;

            // Busca a disponibilidade pelo ID
            var existingAvailability = await _roomAvailabilityRepository.GetByIdAsync(availabilityId);
            if (existingAvailability == null)
            {
                response.Success = false;
                response.Message = "Disponibilidade não encontrada.";
                return response;
            }

            // Atualiza os dados da disponibilidade
            var roomAvailability = _mapper.Map<RoomAvailability>(availabilityDto);
            roomAvailability.Id = availabilityId;

            // Valida a disponibilidade antes de atualizar
            var validationResult = await _entityValidator.ValidateAsync(roomAvailability);
            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return response;
            }

            // Atualiza no banco
            var updatedAvailability = await _repository.UpdateAsync(roomAvailability);

            // Retorna a disponibilidade atualizada no formato DTO
            response.Data = _mapper.Map<RoomAvailabilityDto>(updatedAvailability);
            response.Success = true;
            response.Message = "Disponibilidade atualizada com sucesso.";
            return response;
        }

        /// <summary>
        /// Exclui uma disponibilidade de quarto existente.
        /// </summary>
        public override async Task<ServiceResponse<string>> DeleteAsync(long id)
        {
            var response = new ServiceResponse<string>();

            // Busca a disponibilidade pelo ID
            var existingAvailability = await _roomAvailabilityRepository.GetByIdAsync(id);
            if (existingAvailability == null)
            {
                response.Success = false;
                response.Message = "Disponibilidade não encontrada.";
                return response;
            }

            // Exclui a disponibilidade
            await _repository.DeleteAsync(id);

            response.Success = true;
            response.Message = "Disponibilidade excluída com sucesso.";
            return response;
        }

        /// <summary>
        /// Recupera todas as disponibilidades associadas a um quarto específico.
        /// </summary>
        public async Task<ServiceResponse<RoomAvailabilityDto[]>> GetAvailabilitiesByRoomIdAsync(long roomId)
        {
            var response = new ServiceResponse<RoomAvailabilityDto[]>();

            // Verifica se o quarto existe
            var roomExists = await _roomRepository.ExistsAsync(r => r.Id == roomId);
            if (!roomExists)
            {
                response.Success = false;
                response.Message = "O quarto informado não existe.";
                return response;
            }

            // Busca todas as disponibilidades pelo RoomId
            var availabilities = await _roomAvailabilityRepository.GetAvailabilityByRoomId(roomId);

            // Retorna as disponibilidades no formato DTO
            response.Data = _mapper.Map<RoomAvailabilityDto[]>(availabilities);
            response.Success = true;
            response.Message = "Disponibilidades recuperadas com sucesso.";
            return response;
        }
    }
}
