using AutoMapper;
using HotelWise.Domain.Dto.Enitty.HotelDtos;
using HotelWise.Domain.Mapper;
using HotelWise.Domain.Model.HotelModels;
using Microsoft.Extensions.Logging.Abstractions;

namespace HotelWise.Domain.Tests.Mapper;

public class MappingProfilesTests
{
    // Cenário: todos os profiles de domínio registrados no AutoMapper
    // Objetivo: garantir que a configuração de mapeamento é válida
    [Fact]
    public void MapperConfiguration_AllProfiles_AssertConfigurationIsValid()
    {
        // Arrange
        var config = CreateMapperConfiguration();

        // Act
        var act = () => config.AssertConfigurationIsValid();

        // Assert
        act.Should().NotThrow();
    }

    // Cenário: mapeamento Hotel -> HotelDto
    // Objetivo: garantir que propriedades do hotel são copiadas para o DTO
    [Fact]
    public void Map_HotelToHotelDto_MapsProperties()
    {
        // Arrange
        var mapper = CreateMapperConfiguration().CreateMapper();
        var hotel = new Hotel
        {
            HotelId = 42,
            HotelName = "StayMate Palace",
            Description = "Hotel de luxo",
            Stars = 5,
            InitialRoomPrice = 399.90m,
            City = "Rio de Janeiro",
            StateCode = "RJ"
        };

        // Act
        var dto = mapper.Map<HotelDto>(hotel);

        // Assert
        dto.Should().NotBeNull();
        dto.HotelId.Should().Be(hotel.HotelId);
        dto.HotelName.Should().Be(hotel.HotelName);
        dto.Description.Should().Be(hotel.Description);
        dto.Stars.Should().Be(hotel.Stars);
        dto.InitialRoomPrice.Should().Be(hotel.InitialRoomPrice);
        dto.City.Should().Be(hotel.City);
        dto.StateCode.Should().Be(hotel.StateCode);
    }

    private static MapperConfiguration CreateMapperConfiguration() =>
        new(
            cfg =>
            {
                cfg.AddProfile<HotelMappingProfile>();
                cfg.AddProfile<UserMappingProfile>();
                cfg.AddProfile<RoomMappingProfile>();
                cfg.AddProfile<RoomAvailabilityMappingProfile>();
                cfg.AddProfile<ReservationMappingProfile>();
                cfg.AddProfile<ChatSessionHistoryMappingProfile>();
            },
            NullLoggerFactory.Instance);
}
