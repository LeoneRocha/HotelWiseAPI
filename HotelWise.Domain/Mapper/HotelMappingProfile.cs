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
        CreateMap<Hotel, HotelDto>();

        CreateMap<Hotel, HotelVector>();
        CreateMap<HotelVector, Hotel>();
    }
}
