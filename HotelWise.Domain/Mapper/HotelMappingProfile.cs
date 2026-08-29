using AutoMapper;
using HotelWise.Domain.Dto.Enitty.HotelDtos;
using HotelWise.Domain.Dto.IA.SemanticKernel;
using HotelWise.Domain.Model.HotelModels;

namespace HotelWise.Domain.Mapper;

public class HotelMappingProfile : Profile
{
    public HotelMappingProfile()
    {
        CreateMap<HotelDto, Hotel>();
        CreateMap<Hotel, HotelDto>()
            .ForMember(d => d.IsHotelInVectorStore, opt => opt.Ignore())
            .ForMember(d => d.Score, opt => opt.Ignore());

        CreateMap<Hotel, HotelVector>()
            .ForMember(d => d.DataKey, opt => opt.Ignore())
            .ForMember(d => d.Embedding, opt => opt.Ignore())
            .ForMember(d => d.Score, opt => opt.Ignore());
        CreateMap<HotelVector, Hotel>(MemberList.None);
    }
}
