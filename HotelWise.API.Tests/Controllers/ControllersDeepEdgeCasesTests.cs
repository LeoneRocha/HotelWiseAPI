using HotelWise.API.Controllers;
using HotelWise.API.Controllers.Ai;
using HotelWise.API.Controllers.HotelEndpoints;
using HotelWise.API.Controllers.ReservationEndpoints;
using HotelWise.API.Controllers.RoomAvailabilityEndpoints;
using HotelWise.API.Controllers.RoomEndpoints;
using HotelWise.Domain.Dto;
using HotelWise.Domain.Dto.Enitty;
using HotelWise.Domain.Dto.Enitty.HotelDtos;
using HotelWise.Domain.Dto.IA.SemanticKernel;
using HotelWise.Domain.Interfaces.Entity;
using HotelWise.Domain.Interfaces.Entity.HotelInterfaces.Service;
using Microsoft.AspNetCore.Mvc;

namespace HotelWise.API.Tests.Controllers;

public class ControllersDeepEdgeCasesTests
{
    // Cenário: Tentativa de login com credenciais inválidas em AuthController.
    // Objetivo: Cobrir AuthController.Authenticate retornando Unauthorized.
    [Fact]
    public async Task AuthController_InvalidCredentials_ShouldReturnUnauthorized()
    {
        // Arrange
        var userServiceMock = new Mock<IUserService>();
        userServiceMock
            .Setup(s => s.Login("usuario_errado", "senha_errada"))
            .ReturnsAsync(new ServiceResponse<GetUserAuthenticatedDto>
            {
                Success = false,
                Message = "Credenciais inválidas."
            });

        var controller = new AuthController(userServiceMock.Object);

        // Act
        var result = await controller.Authenticate(new UserLoginDto { Login = "usuario_errado", Password = "senha_errada" });

        // Assert
        result.Result.Should().BeOfType<UnauthorizedObjectResult>();
    }

    // Cenário: Consulta e operações em HotelsController.
    // Objetivo: Cobrir GetById com hotel inexistente (404), Update com ID divergente (400) e SemanticSearch.
    [Fact]
    public async Task HotelsController_EdgeCases_ShouldReturnExpectedResults()
    {
        // Arrange
        var hotelServiceMock = new Mock<IHotelService>();
        var hotelSearchServiceMock = new Mock<IHotelSearchService>();

        hotelServiceMock.Setup(s => s.GetHotelByIdAsync(999)).ReturnsAsync((ServiceResponse<HotelDto?>)null!);

        hotelSearchServiceMock.Setup(s => s.SemanticSearch(It.IsAny<SearchCriteria>()))
            .ReturnsAsync(new ServiceResponse<HotelSemanticResult>
            {
                Success = true,
                Data = new HotelSemanticResult { PromptResultContent = "Resultado da busca" }
            });

        var controller = new HotelsController(hotelServiceMock.Object, hotelSearchServiceMock.Object);
        ControllerTestHelper.SetAuthenticatedUser(controller);

        // Act
        var notFoundGet = await controller.GetById(999);
        var badRequestUpdate = await controller.Update(1, new HotelDto { HotelId = 2 });
        var searchResult = await controller.SemanticSearch(new SearchCriteria());

        // Assert
        Assert.Multiple(() =>
        {
            notFoundGet.Should().BeOfType<NotFoundResult>();
            badRequestUpdate.Should().BeOfType<BadRequestResult>();
            searchResult.Should().BeOfType<OkObjectResult>();
        });
    }

    // Cenário: Consulta de reserva inexistente ou quarto sem reservas em ReservationsController.
    // Objetivo: Cobrir GetById (404) e GetByRoomId (404).
    [Fact]
    public async Task ReservationsController_NotFoundScenarios_ShouldReturnNotFound()
    {
        // Arrange
        var resServiceMock = new Mock<IReservationService>();

        resServiceMock.Setup(s => s.GetByIdAsync(999)).ReturnsAsync((ReservationDto?)null);
        resServiceMock.Setup(s => s.GetReservationsByRoomIdAsync(999))
            .ReturnsAsync(new ServiceResponse<ReservationDto[]>
            {
                Data = []
            });

        var controller = new ReservationsController(resServiceMock.Object);
        ControllerTestHelper.SetAuthenticatedUser(controller);

        // Act
        var failedGet = await controller.GetById(999);
        var failedByRoom = await controller.GetByRoomId(999);

        // Assert
        Assert.Multiple(() =>
        {
            failedGet.Should().BeOfType<NotFoundObjectResult>();
            failedByRoom.Should().BeOfType<NotFoundObjectResult>();
        });
    }

    // Cenário: Disponibilidade não encontrada e batch vazio em RoomAvailabilityController.
    // Objetivo: Cobrir GetById (404) e CreateBatch vazio.
    [Fact]
    public async Task RoomAvailabilityController_EdgeCases_ShouldBehaveCorrectly()
    {
        // Arrange
        var availServiceMock = new Mock<IRoomAvailabilityService>();

        availServiceMock.Setup(s => s.GetByIdAsync(999)).ReturnsAsync((RoomAvailabilityDto?)null);
        availServiceMock.Setup(s => s.DeleteAsync(7)).Returns(Task.CompletedTask);

        var controller = new RoomAvailabilityController(availServiceMock.Object);
        ControllerTestHelper.SetAuthenticatedUser(controller);

        // Act
        var notFound = await controller.GetById(999);
        var emptyBatch = await controller.CreateBatch([]);
        var deleted = await controller.Delete(7);

        // Assert
        Assert.Multiple(() =>
        {
            notFound.Should().BeOfType<NotFoundObjectResult>();
            emptyBatch.Should().BeOfType<OkObjectResult>();
            deleted.Should().BeOfType<OkObjectResult>();
        });
    }

    // Cenário: Quarto inexistente e ID divergente na atualização em RoomsController.
    // Objetivo: Cobrir GetById (404) e Update (400).
    [Fact]
    public async Task RoomsController_EdgeCases_ShouldBehaveCorrectly()
    {
        // Arrange
        var roomServiceMock = new Mock<IRoomService>();

        roomServiceMock.Setup(s => s.GetByIdAsync(999)).ReturnsAsync((RoomDto?)null);
        roomServiceMock.Setup(s => s.GetRoomsByHotelIdAsync(999)).ReturnsAsync(new ServiceResponse<RoomDto[]> { Data = [] });
        roomServiceMock.Setup(s => s.DeleteAsync(3)).Returns(Task.CompletedTask);

        var controller = new RoomsController(roomServiceMock.Object);
        ControllerTestHelper.SetAuthenticatedUser(controller);

        // Act
        var notFound = await controller.GetById(999);
        var roomsEmpty = await controller.GetRoomsByHotel(999);
        var idMismatch = await controller.Update(1, new RoomDto { Id = 2 });
        var deleted = await controller.Delete(3);

        // Assert
        Assert.Multiple(() =>
        {
            notFound.Should().BeOfType<NotFoundObjectResult>();
            roomsEmpty.Should().BeOfType<OkObjectResult>();
            idMismatch.Should().BeOfType<BadRequestObjectResult>();
            deleted.Should().BeOfType<OkObjectResult>();
        });
    }

    // Cenário: Falha em AssistantController (resposta vazia).
    // Objetivo: Cobrir AssistantController.AskAssistant retornando BadRequest.
    [Fact]
    public async Task AssistantController_FailureScenarios_ShouldReturnBadRequest()
    {
        // Arrange
        var assistantServiceMock = new Mock<IAssistantService>();
        assistantServiceMock
            .Setup(s => s.AskAssistant(It.IsAny<AskAssistantRequest>()))
            .ReturnsAsync([]);

        var controller = new AssistantController(assistantServiceMock.Object);
        ControllerTestHelper.SetAuthenticatedUser(controller);

        // Act
        var failedResult = await controller.AskAssistant(new AskAssistantRequest());

        // Assert
        failedResult.Should().BeOfType<BadRequestResult>();
    }
}
