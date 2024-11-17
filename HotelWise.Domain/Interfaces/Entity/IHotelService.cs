using HotelWise.Domain.Dto;
using HotelWise.Domain.Model;

namespace HotelWise.Domain.Interfaces.Entity
{
    public interface IHotelService
    {
        Task AddHotelAsync(HotelDto hotel);
        Task DeleteHotelAsync(long id);
        Task<HotelDto[]> GetAllHotelsAsync();
        Task<HotelDto?> GetHotelByIdAsync(long id);
        Task UpdateHotelAsync(HotelDto hotel);
        Task<HotelDto> GenerateHotelByIA();

        Task<HotelDto[]> SemanticSearch(SearchCriteria searchCriteria);
    } 
}