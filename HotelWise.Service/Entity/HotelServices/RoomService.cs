﻿using AutoMapper;
using FluentValidation;
using HotelWise.Domain.Dto;
using HotelWise.Domain.Dto.Enitty.HotelDtos;
using HotelWise.Service.Generic;
using HotelWise.Domain.Model.HotelModels;
using Serilog;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Repository;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Service;
using HotelWise.Domain.Helpers;

namespace HotelWise.Service.Entity.HotelServices
{
    public class RoomService : GenericEntityServiceBase<Room, RoomDto>, IRoomService
    {
        private readonly IRoomRepository _roomRepository;

        public RoomService(
              ILogger logger,
              IRoomRepository repository,
              IMapper mapper,
              IValidator<Room> entityValidator
        ) : base(repository, mapper, logger, entityValidator)
        {
            _roomRepository = repository;
        }


        /// <summary>
        /// Cria um novo quarto após validação de negócios.
        /// </summary>
        public override async Task<ServiceResponse<RoomDto>> CreateAsync(RoomDto roomDto)
        {
            var response = new ServiceResponse<RoomDto>();

            // Mapeia o DTO para a entidade
            var room = _mapper.Map<Room>(roomDto);

            room.CreatedDate = DataHelper.GetDateTimeNow();
            room.CreatedUserId = base.UserId;
            room.ModifyDate = DataHelper.GetDateTimeNow();
            room.ModifyUserId = base.UserId;

            // Valida o quarto antes de criar
            var validationResult = await _entityValidator.ValidateAsync(room);
            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return response;
            }

            // Insere o quarto no banco
            var createdRoom = await _repository.AddAsync(room);

            // Retorna o quarto criado no formato DTO
            response.Data = _mapper.Map<RoomDto>(createdRoom);
            response.Success = true;
            response.Message = "Quarto criado com sucesso.";
            return response;
        }

        /// <summary>
        /// Atualiza um quarto existente.
        /// </summary>
        public override async Task<ServiceResponse<RoomDto>> UpdateAsync(RoomDto roomDto)
        {
            var response = new ServiceResponse<RoomDto>();
            var roomId = roomDto.Id;

            // Busca o quarto pelo ID
            var existingRoom = await _roomRepository.ExistsAsync(x => x.Id == roomId);
            if (!existingRoom)
            {
                response.Success = false;
                response.Message = "Quarto não encontrado.";
                return response;
            }

            // Atualiza os dados do quarto com os valores do DTO
            var room = _mapper.Map<Room>(roomDto);
            room.Id = roomId;
            room.CreatedDate = DataHelper.GetDateTimeNow();
            room.CreatedUserId = base.UserId;
            room.ModifyDate = DataHelper.GetDateTimeNow();
            room.ModifyUserId = base.UserId;
             
            // Valida o quarto antes de atualizar
            var validationResult = await _entityValidator.ValidateAsync(room);
            if (!validationResult.IsValid)
            {
                response.Success = false;
                response.Message = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return response;
            }

            // Atualiza no banco
            var updatedRoom = await _repository.UpdateAsync(room);

            // Retorna o quarto atualizado no formato DTO
            response.Data = _mapper.Map<RoomDto>(updatedRoom);
            response.Success = true;
            response.Message = "Quarto atualizado com sucesso.";
            return response;
        }

        /// <summary>
        /// Exclui um quarto pelo ID.
        /// </summary>
        public override async Task<ServiceResponse<string>> DeleteAsync(long id)
        {
            var response = new ServiceResponse<string>();

            // Busca o quarto pelo ID
            var existingRoom = await _roomRepository.GetByIdAsync(id);
            if (existingRoom == null)
            {
                response.Success = false;
                response.Message = "Quarto não encontrado.";
                return response;
            }

            // Exclui o quarto
            await _repository.DeleteAsync(id);

            response.Success = true;
            response.Message = "Quarto excluído com sucesso.";
            return response;
        }

        /// <summary>
        /// Recupera todos os quartos associados a um hotel específico.
        /// </summary>
        public async Task<ServiceResponse<RoomDto[]>> GetRoomsByHotelIdAsync(long hotelId)
        {
            var response = new ServiceResponse<RoomDto[]>();

            // Busca todos os quartos associados ao hotel
            var rooms = await _roomRepository.GetRoomsByHotelIdAsync(hotelId);
            if (rooms == null || rooms.Length == 0)
            {
                response.Success = false;
                response.Message = "Nenhum quarto encontrado para o hotel informado.";
                return response;
            }

            // Retorna os quartos no formato DTO
            response.Data = _mapper.Map<RoomDto[]>(rooms);
            response.Success = true;
            response.Message = "Quartos recuperados com sucesso.";
            return response;
        }
    }
}