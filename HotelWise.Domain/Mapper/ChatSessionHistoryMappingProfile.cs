using AutoMapper;
using HotelWise.Domain.Dto.IA;
using HotelWise.Domain.Model.AI;

namespace HotelWise.Domain.Mapper;

public class ChatSessionHistoryMappingProfile : Profile
{
    public ChatSessionHistoryMappingProfile()
    {
        CreateMap<ChatSessionHistory, ChatSessionHistoryDto>();
        CreateMap<ChatSessionHistoryDto, ChatSessionHistory>();
    }
}
