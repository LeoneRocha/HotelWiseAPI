using AutoMapper;
using HotelWise.Domain.Dto.IA;
using HotelWise.Domain.Model.AI;

namespace HotelWise.Domain.Mapper;

/// <summary>
/// Perfil AutoMapper para mapeamento bidirecional entre <see cref="ChatSessionHistory"/> e <see cref="ChatSessionHistoryDto"/>.
/// </summary>
public class ChatSessionHistoryMappingProfile : Profile
{
    /// <summary>
    /// Configura as projeções e transformações de propriedades para histórico de sessões de chat.
    /// </summary>
    public ChatSessionHistoryMappingProfile()
    {
        CreateMap<ChatSessionHistory, ChatSessionHistoryDto>()
            .ForMember(d => d.UpdateDate, opt => opt.Ignore());
        CreateMap<ChatSessionHistoryDto, ChatSessionHistory>();
    }
}
