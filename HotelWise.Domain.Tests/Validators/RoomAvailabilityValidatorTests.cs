using System.Linq.Expressions;
using FluentValidation;
using HotelWise.Domain.Enuns.Hotel;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Repository;
using HotelWise.Domain.Model.HotelModels;
using HotelWise.Domain.Validator.HotelValidators;

namespace HotelWise.Domain.Tests.Validators;

public class RoomAvailabilityValidatorTests
{
    private readonly Mock<IRoomRepository> _roomRepository = new();
    private readonly Mock<IRoomAvailabilityRepository> _roomAvailabilityRepository = new();
    private readonly RoomAvailabilityValidator _validator;

    public RoomAvailabilityValidatorTests()
    {
        _validator = new RoomAvailabilityValidator(
            _roomRepository.Object,
            _roomAvailabilityRepository.Object);
    }

    // Cenário: nova disponibilidade (Id=0) com datas futuras, BRL e um item de preço
    // Objetivo: garantir happy path quando o quarto existe e não há conflitos
    [Fact]
    public async Task ValidateAsync_NewAvailability_HappyPath_Passes()
    {
        // Arrange
        _roomRepository
            .Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Room, bool>>>()))
            .ReturnsAsync(true);

        _roomAvailabilityRepository
            .Setup(r => r.GetAvailabilityByRoomId(It.IsAny<long>()))
            .ReturnsAsync([]);

        var start = DateTime.UtcNow.Date.AddDays(7);
        var availability = new RoomAvailability
        {
            Id = 0,
            RoomId = 5,
            Currency = "BRL",
            StartDate = start,
            EndDate = start.AddDays(5),
            AvailabilityWithPrice =
            [
                new RoomPriceAndAvailabilityItem
                {
                    DayOfWeek = DayOfWeek.Monday,
                    Price = 250m,
                    QuantityAvailable = 3,
                    Currency = "BRL",
                    Status = RoomAvailabilityStatus.Available
                }
            ]
        };

        // Act
        var result = await _validator.ValidateAsync(availability);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    // Cenário: quarto informado não existe
    // Objetivo: garantir falha quando ExistsAsync do quarto retorna false
    [Fact]
    public async Task ValidateAsync_RoomDoesNotExist_Fails()
    {
        // Arrange
        _roomRepository
            .Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Room, bool>>>()))
            .ReturnsAsync(false);

        _roomAvailabilityRepository
            .Setup(r => r.GetAvailabilityByRoomId(It.IsAny<long>()))
            .ReturnsAsync([]);

        var start = DateTime.UtcNow.Date.AddDays(7);
        var availability = new RoomAvailability
        {
            Id = 0,
            RoomId = 999,
            Currency = "BRL",
            StartDate = start,
            EndDate = start.AddDays(3),
            AvailabilityWithPrice =
            [
                new RoomPriceAndAvailabilityItem
                {
                    DayOfWeek = DayOfWeek.Tuesday,
                    Price = 180m,
                    QuantityAvailable = 1,
                    Currency = "BRL",
                    Status = RoomAvailabilityStatus.Available
                }
            ]
        };

        // Act
        var result = await _validator.ValidateAsync(availability);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(RoomAvailability.RoomId));
    }
}
