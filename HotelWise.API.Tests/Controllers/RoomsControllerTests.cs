using HotelWise.API.Controllers.RoomEndpoints;
using HotelWise.Domain.Dto.Enitty.HotelDtos;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Service;
using Microsoft.AspNetCore.Mvc;

namespace HotelWise.API.Tests.Controllers;

public class RoomsControllerTests
{
    private readonly Mock<IRoomService> _roomService = new();
    private readonly RoomsController _controller;

    public RoomsControllerTests()
    {
        _controller = new RoomsController(_roomService.Object);
        ControllerTestHelper.SetAuthenticatedUser(_controller);
    }

    [Fact]
    public async Task GetById_When_Found_Should_Return_Ok()
    {
        // Arrange
        var room = new RoomDto { Id = 1, HotelId = 10 };
        _roomService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(room);

        // Act
        var result = await _controller.GetById(1);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        _roomService.Verify(s => s.SetUserId(1), Times.Once);
        _roomService.Verify(s => s.GetByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task GetById_When_NotFound_Should_Return_NotFound()
    {
        // Arrange
        _roomService.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((RoomDto?)null);

        // Act
        var result = await _controller.GetById(99);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task GetRoomsByHotel_When_Response_Null_Should_Return_NotFound()
    {
        // Arrange
        _roomService.Setup(s => s.GetRoomsByHotelIdAsync(5))
            .ReturnsAsync((ServiceResponse<RoomDto[]>)null!);

        // Act
        var result = await _controller.GetRoomsByHotel(5);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task GetRoomsByHotel_When_Empty_Should_Return_Ok_With_Message()
    {
        // Arrange
        var response = new ServiceResponse<RoomDto[]> { Data = [] };
        _roomService.Setup(s => s.GetRoomsByHotelIdAsync(5)).ReturnsAsync(response);

        // Act
        var result = await _controller.GetRoomsByHotel(5);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        response.Message.Should().Be("Nenhum quarto encontrado para o hotel informado.");
    }

    [Fact]
    public async Task GetRoomsByHotel_When_Has_Data_Should_Return_Ok()
    {
        // Arrange
        var response = new ServiceResponse<RoomDto[]>
        {
            Data = [new RoomDto { Id = 1, HotelId = 5 }]
        };
        _roomService.Setup(s => s.GetRoomsByHotelIdAsync(5)).ReturnsAsync(response);

        // Act
        var result = await _controller.GetRoomsByHotel(5);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        _roomService.Verify(s => s.GetRoomsByHotelIdAsync(5), Times.Once);
    }

    [Fact]
    public async Task Create_Should_Return_Ok()
    {
        // Arrange
        var room = new RoomDto { Id = 1, HotelId = 2 };
        var response = new ServiceResponse<RoomDto> { Data = room };
        _roomService.Setup(s => s.CreateAsync(room)).ReturnsAsync(response);

        // Act
        var result = await _controller.Create(room);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        _roomService.Verify(s => s.CreateAsync(room), Times.Once);
    }

    [Fact]
    public async Task Update_When_Ids_Match_Should_Return_Ok()
    {
        // Arrange
        var room = new RoomDto { Id = 4 };
        var response = new ServiceResponse<RoomDto> { Data = room };
        _roomService.Setup(s => s.UpdateAsync(room)).ReturnsAsync(response);

        // Act
        var result = await _controller.Update(4, room);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        _roomService.Verify(s => s.UpdateAsync(room), Times.Once);
    }

    [Fact]
    public async Task Update_When_Ids_Mismatch_Should_Return_BadRequest()
    {
        // Arrange
        var room = new RoomDto { Id = 2 };

        // Act
        var result = await _controller.Update(1, room);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
        _roomService.Verify(s => s.UpdateAsync(It.IsAny<RoomDto>()), Times.Never);
    }

    [Fact]
    public async Task Delete_Should_Return_Ok()
    {
        // Arrange
        _roomService.Setup(s => s.DeleteAsync(8)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Delete(8);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        _roomService.Verify(s => s.DeleteAsync(8), Times.Once);
    }
}

