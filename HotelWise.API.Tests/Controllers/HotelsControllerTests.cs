using HotelWise.API.Controllers.HotelEndpoints;
using HotelWise.Core.SDK.AI.DTO;
using HotelWise.Core.SDK.Common;
using HotelWise.Domain.Dto.Enitty.HotelDtos;
using HotelWise.Domain.Dto.IA.SemanticKernel;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Service;
using Microsoft.AspNetCore.Mvc;

namespace HotelWise.API.Tests.Controllers;

public class HotelsControllerTests
{
    private static readonly string[] ExpectedTags = ["spa", "pool"];

    private readonly Mock<IHotelService> _hotelService = new();
    private readonly Mock<IHotelSearchService> _hotelSearchService = new();
    private readonly HotelsController _controller;

    public HotelsControllerTests()
    {
        _controller = new HotelsController(_hotelService.Object, _hotelSearchService.Object);
        ControllerTestHelper.SetAuthenticatedUser(_controller);
    }

    [Fact]
    public async Task GetAll_Should_Return_Ok_And_Invoke_Service()
    {
        // Arrange
        var response = new ServiceResponse<HotelDto[]> { Data = [new HotelDto { HotelId = 1 }] };
        _hotelService.Setup(s => s.GetAllHotelsAsync()).ReturnsAsync(response);

        // Act
        var result = await _controller.GetAll();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        _hotelService.Verify(s => s.SetUserId(1), Times.Once);
        _hotelService.Verify(s => s.GetAllHotelsAsync(), Times.Once);
    }

    [Fact]
    public async Task GetById_When_Found_Should_Return_Ok()
    {
        // Arrange
        var response = new ServiceResponse<HotelDto?> { Data = new HotelDto { HotelId = 10 } };
        _hotelService.Setup(s => s.GetHotelByIdAsync(10)).ReturnsAsync(response);

        // Act
        var result = await _controller.GetById(10);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        _hotelService.Verify(s => s.GetHotelByIdAsync(10), Times.Once);
    }

    [Fact]
    public async Task GetById_When_Null_Should_Return_NotFound()
    {
        // Arrange
        _hotelService.Setup(s => s.GetHotelByIdAsync(99))
            .ReturnsAsync((ServiceResponse<HotelDto?>)null!);

        // Act
        var result = await _controller.GetById(99);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetAllTags_Should_Return_Ok()
    {
        // Arrange
        _hotelService.Setup(s => s.GetAllTags()).ReturnsAsync(ExpectedTags);

        // Act
        var result = await _controller.GetAllTags();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        ((OkObjectResult)result).Value.Should().BeEquivalentTo(ExpectedTags);
        _hotelService.Verify(s => s.GetAllTags(), Times.Once);
    }

    [Fact]
    public async Task AddVectorById_Should_Return_Ok()
    {
        // Arrange
        var response = new ServiceResponse<bool> { Data = true };
        _hotelService.Setup(s => s.InsertHotelInVectorStore(5)).ReturnsAsync(response);

        // Act
        var result = await _controller.AddVectorById(5);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        _hotelService.Verify(s => s.InsertHotelInVectorStore(5), Times.Once);
    }

    [Fact]
    public async Task Generate_Should_Return_Ok()
    {
        // Arrange
        var response = new ServiceResponse<HotelDto> { Data = new HotelDto { HotelId = 2 } };
        _hotelService.Setup(s => s.GenerateHotelByIA()).ReturnsAsync(response);

        // Act
        var result = await _controller.Generate();

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        _hotelService.Verify(s => s.GenerateHotelByIA(), Times.Once);
    }

    [Fact]
    public async Task SemanticSearch_Should_Return_Ok()
    {
        // Arrange
        var criteria = new SearchCriteria { SearchTextCriteria = "beach" };
        var response = new ServiceResponse<HotelSemanticResult>
        {
            Data = new HotelSemanticResult { PromptResultContent = "ok" }
        };
        _hotelSearchService.Setup(s => s.SemanticSearch(criteria)).ReturnsAsync(response);

        // Act
        var result = await _controller.SemanticSearch(criteria);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        _hotelService.Verify(s => s.SetUserId(1), Times.Once);
        _hotelSearchService.Verify(s => s.SemanticSearch(criteria), Times.Once);
    }

    [Fact]
    public async Task Create_Should_Return_Ok()
    {
        // Arrange
        var hotel = new HotelDto { HotelId = 1 };
        var response = new ServiceResponse<bool> { Data = true };
        _hotelService.Setup(s => s.AddHotelAsync(hotel)).ReturnsAsync(response);

        // Act
        var result = await _controller.Create(hotel);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        _hotelService.Verify(s => s.AddHotelAsync(hotel), Times.Once);
    }

    [Fact]
    public async Task Update_When_Ids_Match_Should_Return_Ok()
    {
        // Arrange
        var hotel = new HotelDto { HotelId = 7 };
        var response = new ServiceResponse<bool> { Data = true };
        _hotelService.Setup(s => s.UpdateHotelAsync(hotel)).ReturnsAsync(response);

        // Act
        var result = await _controller.Update(7, hotel);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        _hotelService.Verify(s => s.UpdateHotelAsync(hotel), Times.Once);
    }

    [Fact]
    public async Task Update_When_Ids_Mismatch_Should_Return_BadRequest()
    {
        // Arrange
        var hotel = new HotelDto { HotelId = 2 };

        // Act
        var result = await _controller.Update(1, hotel);

        // Assert
        result.Should().BeOfType<BadRequestResult>();
        _hotelService.Verify(s => s.UpdateHotelAsync(It.IsAny<HotelDto>()), Times.Never);
    }

    [Fact]
    public async Task Delete_Should_Return_Ok()
    {
        // Arrange
        var response = new ServiceResponse<bool> { Data = true };
        _hotelService.Setup(s => s.DeleteHotelAsync(3)).ReturnsAsync(response);

        // Act
        var result = await _controller.Delete(3);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        _hotelService.Verify(s => s.DeleteHotelAsync(3), Times.Once);
    }
}
