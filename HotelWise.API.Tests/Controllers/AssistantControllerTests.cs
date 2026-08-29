using HotelWise.API.Controllers.Ai;
using HotelWise.Core.SDK.AI.Abstractions;
using HotelWise.Core.SDK.AI.DTO;
using Microsoft.AspNetCore.Mvc;

namespace HotelWise.API.Tests.Controllers;

public class AssistantControllerTests
{
    private readonly Mock<IAssistantService> _assistantService = new();
    private readonly AssistantController _controller;

    public AssistantControllerTests()
    {
        _controller = new AssistantController(_assistantService.Object);
        ControllerTestHelper.SetAuthenticatedUser(_controller);
    }

    [Fact]
    public async Task AskAssistant_When_Has_Response_Should_Return_Ok()
    {
        // Arrange
        var request = new AskAssistantRequest { Message = "Hello", Token = "t1" };
        var answers = new[]
        {
            new AskAssistantResponse { Message = "Hi", Token = "t1" }
        };
        _assistantService.Setup(s => s.AskAssistant(request)).ReturnsAsync(answers);

        // Act
        var result = await _controller.AskAssistant(request);

        // Assert
        result.Should().BeOfType<OkObjectResult>();
        _assistantService.Verify(s => s.SetUserId(1), Times.Once);
        _assistantService.Verify(s => s.AskAssistant(request), Times.Once);
    }

    [Fact]
    public async Task AskAssistant_When_Empty_Should_Return_BadRequest()
    {
        // Arrange
        var request = new AskAssistantRequest { Message = "Hello", Token = "t1" };
        _assistantService.Setup(s => s.AskAssistant(request))
            .ReturnsAsync(Array.Empty<AskAssistantResponse>());

        // Act
        var result = await _controller.AskAssistant(request);

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }

    [Fact]
    public async Task AskAssistant_When_Null_Should_Return_BadRequest()
    {
        // Arrange
        var request = new AskAssistantRequest { Message = "Hello", Token = "t1" };
        _assistantService.Setup(s => s.AskAssistant(request))
            .ReturnsAsync((AskAssistantResponse[]?)null);

        // Act
        var result = await _controller.AskAssistant(request);

        // Assert
        result.Should().BeOfType<BadRequestResult>();
        _assistantService.Verify(s => s.AskAssistant(request), Times.Once);
    }
}
