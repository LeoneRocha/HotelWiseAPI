using HotelWise.Domain.Dto;
using HotelWise.Domain.Dto.Enitty;
using HotelWise.Domain.Dto.Enitty.HotelDtos;
using HotelWise.Domain.Dto.IA;
using HotelWise.Domain.Dto.IA.SemanticKernel;
using HotelWise.Domain.Enuns.Hotel;
using HotelWise.Domain.Model;
using HotelWise.Domain.Model.AI;
using HotelWise.Domain.Model.HotelModels;

namespace HotelWise.Domain.Tests.Models;

public class DomainDtoAndModelsDeepTests
{
    // Cenário: Instanciação, getters/setters e integridade de DTOs do domínio.
    // Objetivo: Cobrir todos os DTOs em HotelWise.Domain.Dto.
    [Fact]
    public void DomainDtos_ShouldGetAndSetPropertiesCorrectly()
    {
        // Arrange & Act
        var now = DateTime.UtcNow;

        var hotelDto = new HotelDto
        {
            HotelId = 1,
            HotelName = "Hotel Teste",
            Description = "Descricao",
            Tags = ["tag1", "tag2"],
            Stars = 4,
            InitialRoomPrice = 250m,
            ZipCode = "12345-678",
            Location = "Av Paulista",
            City = "Sao Paulo",
            StateCode = "SP",
            IsHotelInVectorStore = true
        };

        var roomDto = new RoomDto
        {
            Id = 10,
            HotelId = 1,
            Name = "Quarto Duplo",
            Description = "Quarto com ar e frigobar",
            Capacity = 2,
            RoomType = RoomType.Double,
            Status = RoomStatus.Available,
            MinimumNights = 1
        };

        var reservationDto = new ReservationDto
        {
            Id = 100,
            RoomId = 10,
            CheckInDate = now.Date.AddDays(1),
            CheckOutDate = now.Date.AddDays(3),
            ReservationDate = now,
            TotalAmount = 500m,
            Currency = "BRL",
            Status = ReservationStatus.Confirmed
        };

        var priceItem = new RoomPriceAndAvailabilityItem
        {
            DayOfWeek = DayOfWeek.Monday,
            Price = 150m,
            Currency = "BRL",
            QuantityAvailable = 5,
            Status = RoomAvailabilityStatus.Available
        };

        var roomAvailDto = new RoomAvailabilityDto
        {
            Id = 20,
            RoomId = 10,
            StartDate = now.Date,
            EndDate = now.Date.AddDays(7),
            Currency = "BRL",
            AvailabilityWithPrice = [priceItem]
        };

        var searchDto = new RoomAvailabilitySearchDto
        {
            HotelId = 1,
            StartDate = now.Date,
            EndDate = now.Date.AddDays(5),
            Currency = "BRL"
        };

        var hotelAvailRequest = new HotelAvailabilityRequestDto
        {
            HotelId = 1,
            StartDate = now.Date,
            EndDate = now.Date.AddDays(5),
            Currency = "BRL"
        };

        var userLoginDto = new UserLoginDto
        {
            Login = "admin",
            Password = "password123"
        };

        var authDto = new GetUserAuthenticatedDto
        {
            Id = 1,
            Name = "Admin User",
            Language = "pt-BR",
            MedicalId = 12345,
            TokenAuth = new TokenVO()
        };

        var chatSessionDto = new ChatSessionHistoryDto
        {
            Id = 50,
            Title = "Ajuda com Reserva",
            IdToken = "token-xyz",
            SessionDateTime = now,
            CountMessages = 4,
            TotalTokensMessage = 320,
            PromptMessageHistory = []
        };

        var hotelInfo = new HotelInfo
        {
            Id = 1,
            IdType = "Hotel"
        };

        var semanticResult = new HotelSemanticResult
        {
            PromptResultContent = "Encontrei opções ótimas!",
            HotelsVectorResult = [hotelDto],
            HotelsIAResult = [hotelDto]
        };

        var hotelVector = new HotelVector
        {
            DataKey = 1,
            HotelName = "Resort Copacabana",
            Description = "Beira mar",
            Tags = ["praia", "piscina"]
        };

        // Assert
        Assert.Multiple(() =>
        {
            hotelDto.HotelName.Should().Be("Hotel Teste");
            hotelDto.IsHotelInVectorStore.Should().BeTrue();
            roomDto.Name.Should().Be("Quarto Duplo");
            reservationDto.TotalAmount.Should().Be(500m);
            priceItem.Status.Should().Be(RoomAvailabilityStatus.Available);
            roomAvailDto.Currency.Should().Be("BRL");
            searchDto.HotelId.Should().Be(1);
            hotelAvailRequest.Currency.Should().Be("BRL");
            userLoginDto.Login.Should().Be("admin");
            authDto.Name.Should().Be("Admin User");
            chatSessionDto.Title.Should().Be("Ajuda com Reserva");
            hotelInfo.IdType.Should().Be("Hotel");
            semanticResult.PromptResultContent.Should().Contain("ótimas");
            hotelVector.HotelName.Should().Be("Resort Copacabana");
        });
    }

    // Cenário: Validação dos valores e integridade de Enums de hotel.
    // Objetivo: Cobrir PaymentMethod, ReservationStatus, RoomAvailabilityStatus, RoomStatus, RoomType.
    [Fact]
    public void DomainEnums_ShouldContainExpectedValues()
    {
        // Assert
        Assert.Multiple(() =>
        {
            Enum.GetValues<PaymentMethod>().Should().NotBeEmpty();
            Enum.GetValues<ReservationStatus>().Should().Contain(ReservationStatus.Confirmed);
            Enum.GetValues<RoomAvailabilityStatus>().Should().Contain(RoomAvailabilityStatus.Available);
            Enum.GetValues<RoomStatus>().Should().Contain(RoomStatus.Available);
            Enum.GetValues<RoomType>().Should().Contain(RoomType.Suite);
        });
    }

    // Cenário: Validação de entidades de domínio (User, Hotel, Room, Reservation, ChatSessionHistory).
    // Objetivo: Cobrir getters, setters e propriedades de auditoria.
    [Fact]
    public void DomainModels_ShouldGetAndSetAuditPropertiesCorrectly()
    {
        // Arrange
        var now = DateTime.UtcNow;
        var user = new User
        {
            Id = 1,
            Name = "Maria",
            Email = "maria@hotelwise.com",
            Login = "maria",
            Role = "Receptionist",
            PasswordHash = [1, 2],
            PasswordSalt = [3, 4],
            RefreshToken = "rt",
            RefreshTokenExpiryTime = now.AddDays(1),
            Enable = true,
            CreatedDate = now,
            ModifyDate = now
        };

        var hotel = new Hotel
        {
            HotelId = 10,
            HotelName = "Pousada Floripa",
            Description = "Charmosa pousada",
            Tags = ["ilha", "natureza"],
            Stars = 4,
            InitialRoomPrice = 180m,
            ZipCode = "88000-000",
            Location = "Lagoa",
            City = "Florianopolis",
            StateCode = "SC",
            CreatedUserId = user.Id,
            CreatedUser = user,
            ModifyUserId = user.Id,
            ModifyUser = user,
            CreatedDate = now,
            ModifyDate = now
        };

        var room = new Room
        {
            Id = 100,
            HotelId = hotel.HotelId,
            Hotel = hotel,
            Name = "Chale Familia",
            Description = "Com hidromassagem",
            Capacity = 4,
            RoomType = RoomType.Family,
            Status = RoomStatus.Available,
            MinimumNights = 2,
            CreatedUserId = user.Id,
            CreatedUser = user,
            ModifyUserId = user.Id,
            ModifyUser = user,
            CreatedDate = now,
            ModifyDate = now
        };

        var reservation = new Reservation
        {
            Id = 500,
            RoomId = room.Id,
            Room = room,
            CheckInDate = now.AddDays(2),
            CheckOutDate = now.AddDays(5),
            ReservationDate = now,
            TotalAmount = 540m,
            Currency = "BRL",
            Status = ReservationStatus.Confirmed,
            UserId = user.Id
        };

        var chatSession = new ChatSessionHistory
        {
            Id = 1,
            Title = "Sessão Inicial",
            IdToken = "session-token-123",
            SessionDateTime = now,
            CountMessages = 2,
            TotalTokensMessage = 90,
            PromptMessageHistory = []
        };

        // Assert
        Assert.Multiple(() =>
        {
            user.Name.Should().Be("Maria");
            hotel.CreatedUser.Should().NotBeNull();
            room.Hotel.Should().NotBeNull();
            reservation.Room.Should().NotBeNull();
            chatSession.CountMessages.Should().Be(2);
        });
    }
}
