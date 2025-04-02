using HotelWise.Domain.Dto;
using HotelWise.Domain.Dto.Enitty.HotelDtos;
using HotelWise.Domain.Interfaces.Base;

namespace HotelWise.Domain.Interfaces.Entity.HotelInterfaces
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