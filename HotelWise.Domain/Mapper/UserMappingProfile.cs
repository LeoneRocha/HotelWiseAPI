using AutoMapper;
using HotelWise.Domain.Dto;
using HotelWise.Domain.Dto.Enitty;
using HotelWise.Domain.Model;

namespace HotelWise.Domain.Mapper;

/// <summary>
/// Perfil AutoMapper para mapeamento entre a entidade <see cref="User"/> e os DTOs de login e usuário autenticado.
/// </summary>
public class UserMappingProfile : Profile
{
    /// <summary>
    /// Configura as regras de mapeamento de usuários e DTOs de autenticação.
    /// </summary>
    public UserMappingProfile()
    {
        CreateMap<User, GetUserAuthenticatedDto>()
            .ForMember(d => d.TokenAuth, opt => opt.Ignore())
            .ForMember(d => d.MedicalId, opt => opt.Ignore());
        CreateMap<UserLoginDto, User>(MemberList.None);
    }
}
