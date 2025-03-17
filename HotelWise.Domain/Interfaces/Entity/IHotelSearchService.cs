using HotelWise.Domain.Dto;
using HotelWise.Domain.Model;
using HotelWise.Service.Generic;

namespace HotelWise.Domain.Interfaces.Entity
{
    public interface IHotelSearchService : IGenericService<HotelDto>
    {
        void SetUserId(long id);

        Task<ServiceResponse<HotelSemanticResult>> SemanticSearch(SearchCriteria searchCriteria);
    }
}