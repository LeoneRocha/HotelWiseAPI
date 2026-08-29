using AutoMapper;
using HotelWise.Domain.Dto.IA;
using HotelWise.Domain.Model.AI;

namespace HotelWise.Domain.Mapper;

public class ChatSessionHistoryMappingProfile : Profile
{
    public ChatSessionHistoryMappingProfile()
    {
        CreateMap<ChatSessionHistory, ChatSessionHistoryDto>()
            .ForMember(d => d.UpdateDate, opt => opt.Ignore());
        CreateMap<ChatSessionHistoryDto, ChatSessionHistory>();
    }
}
