using HotelWise.Domain.Dto;
using HotelWise.Domain.Model;

namespace HotelWise.Domain.Interfaces.Entity
{
    public interface IHotelService
    {
        void SetUserId(long id);
        Task<ServiceResponse<bool>> InsertHotelInVectorStore(long id);
        Task<ServiceResponse<bool>> AddHotelAsync(HotelDto hotel);
        Task<ServiceResponse<bool>> DeleteHotelAsync(long id);
        Task<ServiceResponse<HotelDto[]>> GetAllHotelsAsync();
        Task<ServiceResponse<HotelDto?>> GetHotelByIdAsync(long id);
        Task<ServiceResponse<bool>> UpdateHotelAsync(HotelDto hotel);
        Task<ServiceResponse<HotelDto>> GenerateHotelByIA();

        Task<ServiceResponse<HotelSemanticResult>> SemanticSearch(SearchCriteria searchCriteria);
        Task<string[]> GetAllTags();
    } 
}