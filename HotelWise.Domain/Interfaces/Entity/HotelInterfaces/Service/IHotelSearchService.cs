using HotelWise.Domain.Dto;
using HotelWise.Domain.Dto.Enitty;
using HotelWise.Domain.Dto.Enitty.HotelDtos;
using HotelWise.Domain.Dto.IA.SemanticKernel;
using HotelWise.Domain.Interfaces.Base;

namespace HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Service
{
    public interface IHotelSearchService : IGenericService<HotelDto>
    { 
        Task<ServiceResponse<HotelSemanticResult>> SemanticSearch(SearchCriteria searchCriteria);
    }
}