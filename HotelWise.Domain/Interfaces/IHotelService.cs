using HotelWise.Domain.Dto;
using HotelWise.Domain.Model;

namespace HotelWise.Domain.Interfaces
{
    public interface IHotelService
    {
        Task AddHotelAsync(Hotel hotel);
        Task DeleteHotelAsync(long id);
        Task<Hotel[]> GetAllHotelsAsync();
        Task<Hotel?> GetHotelByIdAsync(long id);
        Task UpdateHotelAsync(Hotel hotel);
        Task<Hotel[]> GenerateHotelsByIA(int numberGerate);

        Task<Hotel[]> SemanticSearch(SearchCriteria searchCriteria);
    }
}