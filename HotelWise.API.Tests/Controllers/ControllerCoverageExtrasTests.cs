using HotelWise.API.Controllers.RoomEndpoints;
using HotelWise.Core.SDK.Common;
using HotelWise.Domain.Dto.Enitty.HotelDtos;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Service;
using Microsoft.AspNetCore.Mvc;

namespace HotelWise.API.Tests.Controllers;

/// <summary>
/// Cobertura extra de ramos/ações residuais dos controllers.
/// </summary>
public class ControllerCoverageExtrasTests
{
    [Fact]
    public async Task Rooms_GetRoomsByHotel_When_Data_Null_Should_Return_Ok_With_Message()
    {
        // Arrange — ramo Data == null (além do Length == 0 já coberto em RoomsControllerTests)
        var roomService = new Mock<IRoomService>();
        var controller = new RoomsController(roomService.Object);
        ControllerTestHelper.SetAuthenticatedUser(controller);

        var response = new ServiceResponse<RoomDto[]> { Data = null };
        roomService.Setup(s => s.GetRoomsByHotelIdAsync(12)).ReturnsAsync(response);

        // Act
        var result = await controller.GetRoomsByHotel(12);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        response.Message.Should().Be("Nenhum quarto encontrado para o hotel informado.");
        roomService.Verify(s => s.GetRoomsByHotelIdAsync(12), Times.Once);
    }
}
