using HotelWise.API.Controllers.RoomAvailabilityEndpoints;
using HotelWise.Core.SDK.Common;
using HotelWise.Domain.Dto.Enitty.HotelDtos;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Service;
using Microsoft.AspNetCore.Mvc;

namespace HotelWise.API.Tests.Controllers;

public class RoomAvailabilityControllerTests
{
    private readonly Mock<IRoomAvailabilityService> _availabilityService = new();
    private readonly RoomAvailabilityController _controller;

    public RoomAvailabilityControllerTests()
    {
        _controller = new RoomAvailabilityController(_availabilityService.Object);
    }

    [Fact]
    public async Task GetById_When_Found_Should_Return_Ok()
    {
        // Arrange
        var availability = new RoomAvailabilityDto { Id = 1, RoomId = 2 };
        _availabilityService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(availability);

        // Act
        var result = await _controller.GetById(1);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        _availabilityService.Verify(s => s.GetByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task GetById_When_NotFound_Should_Return_NotFound()
    {
        // Arrange
        _availabilityService.Setup(s => s.GetByIdAsync(99)).ReturnsAsync((RoomAvailabilityDto?)null);

        // Act
        var result = await _controller.GetById(99);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task GetAvailabilitiesBySearchCriteriaAsync_When_Has_Data_Should_Return_Ok()
    {
        // Arrange
        var search = new RoomAvailabilitySearchDto
        {
            HotelId = 1,
            StartDate = DateTime.UtcNow.Date,
            Currency = "USD"
        };
        var response = new ServiceResponse<RoomAvailabilityDto[]>
        {
            Data = [new RoomAvailabilityDto { Id = 1 }]
        };
        _availabilityService.Setup(s => s.GetAvailabilitiesBySearchCriteriaAsync(search))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.GetAvailabilitiesBySearchCriteriaAsync(search);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        _availabilityService.Verify(s => s.GetAvailabilitiesBySearchCriteriaAsync(search), Times.Once);
    }

    [Fact]
    public async Task GetAvailabilitiesBySearchCriteriaAsync_When_Empty_Should_Return_Ok_With_Message()
    {
        // Arrange
        var search = new RoomAvailabilitySearchDto
        {
            HotelId = 1,
            StartDate = DateTime.UtcNow.Date,
            Currency = "USD"
        };
        var response = new ServiceResponse<RoomAvailabilityDto[]> { Data = [] };
        _availabilityService.Setup(s => s.GetAvailabilitiesBySearchCriteriaAsync(search))
            .ReturnsAsync(response);

        // Act
        var result = await _controller.GetAvailabilitiesBySearchCriteriaAsync(search);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var ok = (OkObjectResult)result;
        var body = ok.Value.Should().BeOfType<ServiceResponse<RoomAvailabilityDto[]>>().Subject;
        body.Message.Should().Be("Nenhuma disponibilidade encontrada para o quarto informado.");
    }

    [Fact]
    public async Task GetAvailabilitiesBySearchCriteriaAsync_When_Null_Should_Return_Ok_With_Message()
    {
        // Arrange
        var search = new RoomAvailabilitySearchDto
        {
            HotelId = 1,
            StartDate = DateTime.UtcNow.Date,
            Currency = "USD"
        };
        _availabilityService.Setup(s => s.GetAvailabilitiesBySearchCriteriaAsync(search))
            .ReturnsAsync((ServiceResponse<RoomAvailabilityDto[]>)null!);

        // Act
        var result = await _controller.GetAvailabilitiesBySearchCriteriaAsync(search);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        var ok = (OkObjectResult)result;
        var body = ok.Value.Should().BeOfType<ServiceResponse<RoomAvailabilityDto[]>>().Subject;
        body.Message.Should().Be("Nenhuma disponibilidade encontrada para o quarto informado.");
    }

    [Fact]
    public async Task CreateBatch_When_Has_Items_Should_Return_Ok()
    {
        // Arrange
        var items = new[] { new RoomAvailabilityDto { Id = 1, RoomId = 2 } };
        var response = new ServiceResponse<string> { Data = "ok" };
        _availabilityService.Setup(s => s.CreateBatchAsync(items)).ReturnsAsync(response);

        // Act
        var result = await _controller.CreateBatch(items);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        _availabilityService.Verify(s => s.CreateBatchAsync(items), Times.Once);
    }

    [Fact]
    public async Task CreateBatch_When_Empty_Should_Return_Ok_Without_Service_Call()
    {
        // Act
        var result = await _controller.CreateBatch([]);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        _availabilityService.Verify(s => s.CreateBatchAsync(It.IsAny<RoomAvailabilityDto[]>()), Times.Never);
    }

    [Fact]
    public async Task CreateBatch_When_Null_Should_Return_Ok_Without_Service_Call()
    {
        // Act
        var result = await _controller.CreateBatch(null!);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        _availabilityService.Verify(s => s.CreateBatchAsync(It.IsAny<RoomAvailabilityDto[]>()), Times.Never);
    }

    [Fact]
    public async Task Delete_Should_Return_Ok()
    {
        // Arrange
        _availabilityService.Setup(s => s.DeleteAsync(4)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.Delete(4);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        _availabilityService.Verify(s => s.DeleteAsync(4), Times.Once);
    }
}
