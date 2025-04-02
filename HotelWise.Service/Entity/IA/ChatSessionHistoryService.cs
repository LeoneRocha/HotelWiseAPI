using AutoMapper;
using FluentValidation;
using HotelWise.Domain.Dto.IA;
using HotelWise.Domain.Dto.SemanticKernel;
using HotelWise.Domain.Interfaces.AppConfig;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces;
using HotelWise.Domain.Interfaces.Entity.IA;
using HotelWise.Domain.Interfaces.SemanticKernel;
using HotelWise.Domain.Model.AI;
using HotelWise.Service.Generic;

namespace HotelWise.Service.Entity
{
    public class ChatSessionHistoryService : GenericEntityServiceBase<ChatSessionHistory, ChatSessionHistoryDto>, IChatSessionHistoryService
    {
        private readonly IChatSessionHistoryRepository _entitylRepository;

        public ChatSessionHistoryService(
            Serilog.ILogger logger,
            IMapper mapper,
            IApplicationIAConfig applicationConfig,
            IChatSessionHistoryRepository entitylRepository,
            IGenerateHotelService generateHotelService,
            IVectorStoreService<HotelVector> hotelVectorStoreService,
            IValidator<ChatSessionHistory> _entityValidator)
            : base(entitylRepository, mapper, logger, _entityValidator)
        {
            _entitylRepository = entitylRepository;
        }

        public async Task DeleteByIdTokenAsync(string token)
        {
            await _entitylRepository.DeleteByIdTokenAsync(token);
        }

        public async Task<ChatSessionHistoryDto?> GetByIdTokenAsync(string token)
        {
            var result = await _entitylRepository.GetByIdTokenAsync(token);
            if (result == null) { return null; }
            var resultDto = _mapper.Map<ChatSessionHistoryDto>(result);
            return resultDto;
        }
    }
}