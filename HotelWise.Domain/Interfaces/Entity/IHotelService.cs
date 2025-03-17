using HotelWise.Domain.Dto;
using HotelWise.Domain.Model;
using HotelWise.Service.Generic;

namespace HotelWise.Domain.Interfaces.Entity
{
    public interface IHotelService : IGenericService<HotelDto>
    {
        void SetUserId(long id);
        Task<ServiceResponse<bool>> InsertHotelInVectorStore(long id);
        Task<ServiceResponse<bool>> AddHotelAsync(HotelDto hotelDto);
        Task<ServiceResponse<bool>> DeleteHotelAsync(long id);
        Task<ServiceResponse<HotelDto[]>> GetAllHotelsAsync();
        Task<ServiceResponse<HotelDto?>> GetHotelByIdAsync(long id);
        Task<ServiceResponse<bool>> UpdateHotelAsync(HotelDto hotelDto);
        Task<ServiceResponse<HotelDto>> GenerateHotelByIA(); 
        Task<string[]> GetAllTags();
    } 
}