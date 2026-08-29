using System.Linq.Expressions;
using FluentValidation;
using HotelWise.Domain.Enuns.Hotel;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Repository;
using HotelWise.Domain.Model.HotelModels;
using HotelWise.Domain.Validator.HotelValidators;

namespace HotelWise.Domain.Tests.Validators;

public class ReservationValidatorTests
{
    private readonly Mock<IRoomRepository> _roomRepository = new();
    private readonly Mock<IRoomAvailabilityRepository> _roomAvailabilityRepository = new();
    private readonly ReservationValidator _validator;

    public ReservationValidatorTests()
    {
        _validator = new ReservationValidator(
            _roomRepository.Object,
            _roomAvailabilityRepository.Object);
    }

    // Cenário: reserva com Room carregado (Available), mínimo de noites e disponibilidade por noite
    // Objetivo: garantir que uma reserva válida passa em todas as regras do ReservationValidator
    [Fact]
    public async Task ValidateAsync_ValidReservation_RoomAvailable_Passes()
    {
        // Arrange
        var checkIn = DateTime.UtcNow.Date.AddDays(10);
        var checkOut = checkIn.AddDays(2);

        var room = new Room
        {
            Id = 1,
            HotelId = 1,
            Name = "Suite Premium",
            Description = "Suite com varanda",
            RoomType = RoomType.Suite,
            Capacity = 2,
            Status = RoomStatus.Available,
            MinimumNights = 1
        };

        var reservation = new Reservation
        {
            RoomId = room.Id,
            Room = room,
            CheckInDate = checkIn,
            CheckOutDate = checkOut,
            // ReservationValidator captura UtcNow no ctor (LessThanOrEqualTo estático);
            // use data claramente no passado para não falhar por timing.
            ReservationDate = DateTime.UtcNow.AddHours(-1),
            TotalAmount = 500m,
            Currency = "BRL",
            Status = ReservationStatus.Confirmed
        };

        _roomRepository
            .Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Room, bool>>>()))
            .ReturnsAsync(true);

        _roomAvailabilityRepository
            .Setup(r => r.GetAvailabilityByDateRange(
                It.IsAny<long>(),
                It.IsAny<DateTime>(),
                It.IsAny<DateTime>()))
            .ReturnsAsync(
            [
                new RoomAvailability
                {
                    Id = 1,
                    RoomId = room.Id,
                    Currency = "BRL",
                    StartDate = checkIn,
                    EndDate = checkOut,
                    Room = room,
                    AvailabilityWithPrice =
                    [
                        new RoomPriceAndAvailabilityItem
                        {
                            DayOfWeek = checkIn.DayOfWeek,
                            Price = 250m,
                            QuantityAvailable = 2,
                            Currency = "BRL",
                            Status = RoomAvailabilityStatus.Available
                        }
                    ]
                }
            ]);

        // Act
        var result = await _validator.ValidateAsync(reservation);

        // Assert
        result.Errors.Should().BeEmpty();
        result.IsValid.Should().BeTrue();
    }
}
