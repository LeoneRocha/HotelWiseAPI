using HotelWise.API.Controllers.ReservationEndpoints;
using HotelWise.Domain.Dto.Enitty.HotelDtos;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Service;
using Microsoft.AspNetCore.Mvc;

namespace HotelWise.API.Tests.Controllers;

public class ReservationsControllerTests
{
    private readonly Mock<IReservationService> _reservationService = new();
    private readonly ReservationsController _controller;

    public ReservationsControllerTests()
    {
        _controller = new ReservationsController(_reservationService.Object);
        ControllerTestHelper.SetAuthenticatedUser(_controller);
    }

    [Fact]
    public async Task GetById_When_Found_Should_Return_Ok()
    {
        // Arrange
        var reservation = new ReservationDto { Id = 1, RoomId = 10 };
        _reservationService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(reservation);

        // Act
        var result = await _controller.GetById(1);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        _reservationService.Verify(s => s.SetUserId(1), Times.Once);
        _reservationService.Verify(s => s.GetByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task GetById_When_NotFound_Should_Return_NotFound()
    {
        // Arrange
        _reservationService.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((ReservationDto?)null);

        // Act
        var result = await _controller.GetById(99);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task Create_Should_Return_Ok()
    {
        // Arrange
        var dto = new ReservationDto { Id = 1, RoomId = 2 };
        var response = new ServiceResponse<ReservationDto> { Data = dto };
        _reservationService.Setup(s => s.CreateAsync(dto)).ReturnsAsync(response);

        // Act
        var result = await _controller.Create(dto);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        _reservationService.Verify(s => s.CreateAsync(dto), Times.Once);
    }

    [Fact]
    public async Task Cancel_Should_Return_Ok()
    {
        // Arrange
        var response = new ServiceResponse<string> { Data = "cancelled" };
        _reservationService.Setup(s => s.CancelReservationAsync(3)).ReturnsAsync(response);

        // Act
        var result = await _controller.Cancel(3);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        _reservationService.Verify(s => s.CancelReservationAsync(3), Times.Once);
    }

    [Fact]
    public async Task GetByRoomId_When_Has_Data_Should_Return_Ok()
    {
        // Arrange
        var response = new ServiceResponse<ReservationDto[]>
        {
            Data = [new ReservationDto { Id = 1, RoomId = 7 }]
        };
        _reservationService.Setup(s => s.GetReservationsByRoomIdAsync(7)).ReturnsAsync(response);

        // Act
        var result = await _controller.GetByRoomId(7);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        _reservationService.Verify(s => s.GetReservationsByRoomIdAsync(7), Times.Once);
    }

    [Fact]
    public async Task GetByRoomId_When_Empty_Should_Return_NotFound()
    {
        // Arrange
        var response = new ServiceResponse<ReservationDto[]> { Data = [] };
        _reservationService.Setup(s => s.GetReservationsByRoomIdAsync(7)).ReturnsAsync(response);

        // Act
        var result = await _controller.GetByRoomId(7);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task GetByRoomId_When_Data_Null_Should_Return_NotFound()
    {
        // Arrange
        var response = new ServiceResponse<ReservationDto[]> { Data = null };
        _reservationService.Setup(s => s.GetReservationsByRoomIdAsync(8)).ReturnsAsync(response);

        // Act
        var result = await _controller.GetByRoomId(8);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }
}

