using System.Linq.Expressions;
using FluentValidation;
using HotelWise.Domain.Enuns.Hotel;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Repository;
using HotelWise.Domain.Model;
using HotelWise.Domain.Model.AI;
using HotelWise.Domain.Model.HotelModels;
using HotelWise.Domain.Validator;
using HotelWise.Domain.Validator.AI;
using HotelWise.Domain.Validator.HotelValidators;

namespace HotelWise.Domain.Tests.Validators;

public class ExhaustiveValidatorsTests
{
    // Cenário: HotelValidator com campos inválidos (nome vazio, estrelas fora de 1-5, preço zerado ou negativo).
    // Objetivo: Garantir que HotelValidator falhe em todas as regras de validação.
    [Fact]
    public async Task HotelValidator_InvalidProperties_ShouldFailValidation()
    {
        // Arrange
        var validator = new HotelValidator();
        var invalidHotel = new Hotel
        {
            HotelName = "", // Inválido
            Description = "Descricao valida",
            Stars = 6, // Inválido (máximo 5)
            InitialRoomPrice = -10, // Inválido
            Tags = []
        };

        // Act
        var result = await validator.ValidateAsync(invalidHotel);

        // Assert
        Assert.Multiple(() =>
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(Hotel.HotelName));
            result.Errors.Should().Contain(e => e.PropertyName == nameof(Hotel.Stars));
            result.Errors.Should().Contain(e => e.PropertyName == nameof(Hotel.InitialRoomPrice));
        });
    }

    // Cenário: RoomValidator com quarto sem hotel associado ou capacidade inválida.
    // Objetivo: Cobrir falhas de validação em RoomValidator.
    [Fact]
    public async Task RoomValidator_InvalidRoom_ShouldFailValidation()
    {
        // Arrange
        var roomRepoMock = new Mock<IRoomRepository>();
        var hotelRepoMock = new Mock<IHotelRepository>();

        hotelRepoMock.Setup(h => h.ExistsAsync(It.IsAny<Expression<Func<Hotel, bool>>>())).ReturnsAsync(false);

        var validator = new RoomValidator(roomRepoMock.Object, hotelRepoMock.Object);

        var invalidRoom = new Room
        {
            HotelId = 999,
            Name = "", // Inválido
            Capacity = 0, // Inválido
            MinimumNights = 0 // Inválido
        };

        // Act
        var result = await validator.ValidateAsync(invalidRoom);

        // Assert
        Assert.Multiple(() =>
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(Room.Name));
            result.Errors.Should().Contain(e => e.PropertyName == nameof(Room.Capacity));
            result.Errors.Should().Contain(e => e.PropertyName == nameof(Room.MinimumNights));
        });
    }

    // Cenário: ReservationValidator com datas inválidas (CheckIn no passado ou CheckOut antes de CheckIn).
    // Objetivo: Cobrir falhas de validação em ReservationValidator.
    [Fact]
    public async Task ReservationValidator_InvalidDates_ShouldFailValidation()
    {
        // Arrange
        var roomRepoMock = new Mock<IRoomRepository>();
        var roomAvailRepoMock = new Mock<IRoomAvailabilityRepository>();

        roomRepoMock.Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Room, bool>>>())).ReturnsAsync(true);

        var validator = new ReservationValidator(roomRepoMock.Object, roomAvailRepoMock.Object);

        var room = new Room
        {
            Id = 1,
            HotelId = 1,
            Name = "Room 1",
            Capacity = 2,
            Status = RoomStatus.Available,
            MinimumNights = 1
        };

        var invalidReservation = new Reservation
        {
            RoomId = room.Id,
            Room = room,
            CheckInDate = DateTime.UtcNow.AddDays(10),
            CheckOutDate = DateTime.UtcNow.AddDays(5), // CheckOut antes de CheckIn
            TotalAmount = -50,
            Currency = ""
        };

        // Act
        var result = await validator.ValidateAsync(invalidReservation);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }

    // Cenário: RoomAvailabilityValidator com intervalo de datas inválido ou moeda vazia.
    // Objetivo: Cobrir falhas de validação em RoomAvailabilityValidator.
    [Fact]
    public async Task RoomAvailabilityValidator_InvalidInterval_ShouldFailValidation()
    {
        // Arrange
        var roomRepoMock = new Mock<IRoomRepository>();
        var availRepoMock = new Mock<IRoomAvailabilityRepository>();

        var validator = new RoomAvailabilityValidator(roomRepoMock.Object, availRepoMock.Object);

        var invalidAvail = new RoomAvailability
        {
            RoomId = 1,
            StartDate = DateTime.UtcNow.AddDays(10),
            EndDate = DateTime.UtcNow.AddDays(5), // EndDate < StartDate
            Currency = ""
        };

        // Act
        var result = await validator.ValidateAsync(invalidAvail);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }

    // Cenário: UserValidator com email inválido e campos vazios.
    // Objetivo: Cobrir falhas de validação em UserValidator.
    [Fact]
    public async Task UserValidator_InvalidUser_ShouldFailValidation()
    {
        // Arrange
        var validator = new UserValidator();
        var invalidUser = new User
        {
            Name = "",
            Email = "email-invalido",
            Login = "",
            Role = ""
        };

        // Act
        var result = await validator.ValidateAsync(invalidUser);

        // Assert
        Assert.Multiple(() =>
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(User.Name));
            result.Errors.Should().Contain(e => e.PropertyName == nameof(User.Email));
            result.Errors.Should().Contain(e => e.PropertyName == nameof(User.Login));
        });
    }

    // Cenário: ChatSessionHistoryValidator com título vazio ou contagem de mensagens negativa.
    // Objetivo: Cobrir falhas de validação em ChatSessionHistoryValidator.
    [Fact]
    public async Task ChatSessionHistoryValidator_InvalidHistory_ShouldFailValidation()
    {
        // Arrange
        var validator = new ChatSessionHistoryValidator();
        var invalidHistory = new ChatSessionHistory
        {
            Title = "",
            IdToken = "",
            CountMessages = -1,
            TotalTokensMessage = -10
        };

        // Act
        var result = await validator.ValidateAsync(invalidHistory);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().NotBeEmpty();
    }
}
