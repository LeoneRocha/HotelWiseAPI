using HotelWise.Domain.Dto;
using HotelWise.Domain.Interfaces.Base;
using HotelWise.Domain.Model;

namespace HotelWise.Domain.Interfaces.Entity.HotelInterfaces
{
    public interface IHotelSearchService : IGenericService<HotelDto>
    {
        void SetUserId(long id);

        Task<ServiceResponse<HotelSemanticResult>> SemanticSearch(SearchCriteria searchCriteria);
    }
}