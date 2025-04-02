using AutoMapper;
using HotelWise.Domain.Dto;
using HotelWise.Domain.Dto.Enitty.HotelDtos;
using HotelWise.Domain.Dto.Enitty;
using HotelWise.Domain.Dto.IA.SemanticKernel;
using HotelWise.Domain.Dto.IA;
using HotelWise.Domain.Model.AI;
using HotelWise.Domain.Model;
using HotelWise.Domain.Model.HotelModels;

namespace HotelWise.Domain.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region USER
            CreateMap<User, GetUserAuthenticatedDto>();
            CreateMap<UserLoginDto, User>();
            #endregion USER

            #region Hotel
            CreateMap<HotelDto, Hotel>();
            CreateMap<Hotel, HotelDto>();

            CreateMap<Hotel, HotelVector>();
            CreateMap<HotelVector, Hotel>();

            CreateMap<ChatSessionHistory, ChatSessionHistoryDto>();
            CreateMap<ChatSessionHistoryDto, ChatSessionHistory>();
            #endregion Hotel

            #region Reservation
            CreateMap<Reservation, ReservationDto>()
                .ForMember(dest => dest.RoomDetails, opt => opt.MapFrom(src => src.Room)); // Inclui detalhes do quarto na reserva
            CreateMap<ReservationDto, Reservation>();
            #endregion Reservation

            #region Room
            CreateMap<Room, RoomDto>()
                .ForMember(dest => dest.Availabilities, opt => opt.MapFrom(src => src.RoomAvailabilities)); // Mapeia as disponibilidades do quarto
            CreateMap<RoomDto, Room>();
            #endregion Room

            #region RoomAvailability
            CreateMap<RoomAvailability, RoomAvailabilityDto>()
                .ForMember(dest => dest.AvailabilityWithPrice, opt => opt.MapFrom(src => src.AvailabilityWithPrice)); // Mapeia preços e disponibilidade
            CreateMap<RoomAvailabilityDto, RoomAvailability>();
             
            #endregion RoomAvailability
        }
    }
}
