using AutoMapper;
using FluentValidation;
using HotelWise.Domain.Dto;
using HotelWise.Domain.Dto.SemanticKernel;
using HotelWise.Domain.Helpers;
using HotelWise.Domain.Interfaces;
using HotelWise.Domain.Interfaces.Entity;
using HotelWise.Domain.Interfaces.SemanticKernel;
using HotelWise.Domain.Model;
using HotelWise.Domain.Model.HotelModels;
using HotelWise.Service.Generic;
using System.Collections.Concurrent;

namespace HotelWise.Service.Entity
{
    public class HotelService : GenericEntityServiceBase<Hotel, HotelDto>, IHotelService
    {
        private readonly IGenerateHotelService _generateHotelService;
        private readonly IVectorStoreService<HotelVector> _hotelVectorStoreService; 
        private readonly IHotelRepository _hotelRepository;
        public HotelService(
            Serilog.ILogger logger,
            IMapper mapper,
            IApplicationIAConfig applicationConfig,
            IHotelRepository hotelRepository,
            IGenerateHotelService generateHotelService,
            IVectorStoreService<HotelVector> hotelVectorStoreService,
            IValidator<Hotel> entityValidator)
            : base(hotelRepository, mapper, logger, entityValidator)
        { 
            _generateHotelService = generateHotelService;
            _hotelVectorStoreService = hotelVectorStoreService;
            _hotelRepository = hotelRepository;
        }
        public async Task<ServiceResponse<HotelDto[]>> GetAllHotelsAsync()
        {
            ServiceResponse<HotelDto[]> response = new ServiceResponse<HotelDto[]>();
            try
            {
                var hotels = await _hotelRepository.GetAllAsync();
                var hotelDtos = _mapper.Map<HotelDto[]>(hotels);

                response.Data = hotelDtos.OrderBy(h => h.HotelName).ToArray();
                response.Success = true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "GetAllHotelsAsync: {Message} at: {time}", ex.Message, DataHelper.GetDateTimeNowToLog());
                response.Errors.Add(new ErrorResponse() { Message = ex.Message });
            }
            return response;
        }
        public async Task<ServiceResponse<bool>> InsertHotelInVectorStore(long id)
        {
            ServiceResponse<bool> response = new ServiceResponse<bool>();
            try
            {
                var hotel = await _hotelRepository.GetByIdAsync(id);

                var hotelDto = _mapper.Map<HotelDto>(hotel);

                await addOrUpdateDataVector(hotelDto);
                response.Success = true;
                response.Data = true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "InsertHotelInVectorStore: {Message} at: {time}", ex.Message, DataHelper.GetDateTimeNowToLog());
                response.Errors.Add(new ErrorResponse() { Message = ex.Message });
            }
            return response;
        }
        public async Task<ServiceResponse<HotelDto?>> GetHotelByIdAsync(long id)
        {
            ServiceResponse<HotelDto?> response = new ServiceResponse<HotelDto?>();
            try
            {
                var hotel = await _hotelRepository.GetByIdAsync(id);

                var hotelDto = _mapper.Map<HotelDto?>(hotel);

                var hoteVector = await _hotelVectorStoreService.GetById(id);

                if (hoteVector != null && hotelDto != null)
                {
                    hotelDto.IsHotelInVectorStore = true;
                }
                response.Success = true;
                response.Data = hotelDto;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "GetHotelByIdAsync: {Message} at: {time}", ex.Message, DataHelper.GetDateTimeNowToLog());
                response.Errors.Add(new ErrorResponse() { Message = ex.Message });
            }
            return response;
        }
        public async Task<ServiceResponse<HotelDto>> GenerateHotelByIA()
        {
            ServiceResponse<HotelDto> response = new ServiceResponse<HotelDto>();
            try
            {
                var hotel = await _generateHotelService.GetHotelAsync();

                var hotelDto = _mapper.Map<HotelDto>(hotel);

                response.Success = true;
                response.Data = hotelDto;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "GenerateHotelByIA: {Message} at: {time}", ex.Message, DataHelper.GetDateTimeNowToLog());
                response.Errors.Add(new ErrorResponse() { Message = ex.Message });
            }
            return response;
        }
        public async Task<ServiceResponse<bool>> AddHotelAsync(HotelDto hotelDto)
        {
            ServiceResponse<bool> response = new ServiceResponse<bool>();
            try
            {
                var hotel = _mapper.Map<Hotel>(hotelDto);

                #region Set default fields for bussines
                hotel.CreatedUserId = UserId;
                hotel.CreatedDate = DataHelper.GetDateTimeNow();
                hotel.ModifyDate = DataHelper.GetDateTimeNow();
                #endregion Set default fields for bussines

                handleTagsBeforeSave(hotel);

                await _hotelRepository.AddAsync(hotel);

                hotelDto = _mapper.Map<HotelDto>(hotel);

                //Add Vector
                await addOrUpdateDataVector(hotelDto);

                response.Success = true;
                response.Data = true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "AddHotelAsync: {Message} at: {time}", ex.Message, DataHelper.GetDateTimeNowToLog());
                response.Errors.Add(new ErrorResponse() { Message = ex.Message });
            }
            return response;
        }
        public async Task<ServiceResponse<bool>> UpdateHotelAsync(HotelDto hotelDto)
        {
            ServiceResponse<bool> response = new ServiceResponse<bool>();
            try
            {
                var hotel = _mapper.Map<Hotel>(hotelDto);

                //Padronizar tags
                handleTagsBeforeSave(hotel);

                #region Set default fields for bussines
                hotel.ModifyUserId = UserId;
                hotel.ModifyDate = DataHelper.GetDateTimeNow();
                #endregion Set default fields for bussines

                await _hotelRepository.UpdateAsync(hotel);

                await addOrUpdateDataVector(hotelDto);

                response.Success = true;
                response.Data = true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "UpdateHotelAsync: {Message} at: {time}", ex.Message, DataHelper.GetDateTimeNowToLog());
                response.Errors.Add(new ErrorResponse() { Message = ex.Message });
            }
            return response;
        }
        public async Task<string[]> GetAllTags()
        {
            List<string> tagsResult = new List<string>();
            try
            {
                int batchSize = 10;
                var allTagsConcurrentBag = new ConcurrentBag<List<string>>();

                int totalHotels = await _hotelRepository.GetTotalHotelsCountAsync();
                int fromCount = 0;
                int toCount = (totalHotels + batchSize - 1) / batchSize;

                await Parallel.ForEachAsync(Enumerable.Range(fromCount, toCount - fromCount), async (index, cancellationToken) =>
                {
                    var tags = await _hotelRepository.GetAllTagsAsync(index * batchSize, batchSize);

                    foreach (var tag in tags)
                    {
                        allTagsConcurrentBag.Add(tag.Select(x => x.ToLower()).ToList());
                    }
                });

                foreach (var tagsbag in allTagsConcurrentBag)
                {
                    tagsResult.AddRange(tagsbag);
                }
                var result = tagsResult.Distinct().OrderBy(tag => tag).ToList();
                tagsResult = result.ToList();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "FetchHotelsAsync: {Message} at: {time}", ex.Message, DataHelper.GetDateTimeNowToLog());
            }
            return tagsResult.ToArray();
        }
        public async Task<ServiceResponse<bool>> DeleteHotelAsync(long id)
        {
            ServiceResponse<bool> response = new ServiceResponse<bool>();
            try
            {
                await _hotelRepository.DeleteAsync(id);

                await _hotelVectorStoreService.DeleteAsync(id);

                response.Success = true;
                response.Data = true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "DeleteHotelAsync: {Message} at: {time}", ex.Message, DataHelper.GetDateTimeNowToLog());
                response.Errors.Add(new ErrorResponse() { Message = ex.Message });
            }
            return response;
        }
         
        private static void handleTagsBeforeSave(Hotel hotel)
        {
            hotel.Tags = hotel.Tags.Select(t => t.ToLower().Trim()).ToArray();
        }
        private async Task addOrUpdateDataVector(HotelDto hotelDto)
        {
            if (hotelDto != null)
            {
                try
                {
                    await _hotelVectorStoreService.UpsertDataAsync(convertHotelToVector(hotelDto));
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "addOrUpdateDataVector: {Message} at: {time}", ex.Message, DataHelper.GetDateTimeNowToLog());
                }
            }
        }         
        private static HotelVector convertHotelToVector(HotelDto hotel)
        {
            return new HotelVector()
            {
                DataKey = (ulong)hotel.HotelId,
                Description = hotel.Description,
                HotelName = hotel.HotelName,
                Tags = hotel.Tags.ToList()
            };
        }
    }
}