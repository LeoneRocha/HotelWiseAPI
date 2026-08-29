using System.Linq.Expressions;
using FluentValidation;
using HotelWise.Domain.Enuns.Hotel;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Repository;
using HotelWise.Domain.Model.HotelModels;
using HotelWise.Domain.Validator.HotelValidators;

namespace HotelWise.Domain.Tests.Validators;

public class RoomValidatorTests
{
    private readonly Mock<IRoomRepository> _roomRepository = new();
    private readonly Mock<IHotelRepository> _hotelRepository = new();
    private readonly RoomValidator _validator;

    public RoomValidatorTests()
    {
        _validator = new RoomValidator(_roomRepository.Object, _hotelRepository.Object);
    }

    // Cenário: hotel existe e quarto com campos válidos
    // Objetivo: garantir que a validação passa quando ExistsAsync retorna true
    [Fact]
    public async Task ValidateAsync_ValidRoom_HotelExists_Passes()
    {
        // Arrange
        _hotelRepository
            .Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Hotel, bool>>>()))
            .ReturnsAsync(true);

        var created = DateTime.UtcNow.AddDays(-1);
        var room = CreateValidRoom(created);

        // Act
        var result = await _validator.ValidateAsync(room);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
        _hotelRepository.Verify(r => r.ExistsAsync(It.IsAny<Expression<Func<Hotel, bool>>>()), Times.AtLeastOnce);
    }

    // Cenário: hotel informado não existe no repositório
    // Objetivo: garantir falha quando ExistsAsync retorna false
    [Fact]
    public async Task ValidateAsync_HotelDoesNotExist_Fails()
    {
        // Arrange
        _hotelRepository
            .Setup(r => r.ExistsAsync(It.IsAny<Expression<Func<Hotel, bool>>>()))
            .ReturnsAsync(false);

        var created = DateTime.UtcNow.AddDays(-1);
        var room = CreateValidRoom(created);

        // Act
        var result = await _validator.ValidateAsync(room);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(Room.HotelId));
    }

    private static Room CreateValidRoom(DateTime created) => new()
    {
        HotelId = 10,
        RoomType = RoomType.Double,
        Capacity = 2,
        Name = "Quarto 101",
        Description = "Quarto duplo com vista para o jardim",
        Status = RoomStatus.Available,
        MinimumNights = 1,
        CreatedDate = created,
        ModifyDate = created.AddHours(1)
    };
}
