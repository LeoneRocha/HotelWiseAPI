using HotelWise.API.Controllers;
using HotelWise.Domain.Dto;
using HotelWise.Domain.Dto.Enitty;
using HotelWise.Domain.Interfaces.Entity;
using Microsoft.AspNetCore.Mvc;

namespace HotelWise.API.Tests.Controllers;

public class AuthControllerTests
{
    private readonly Mock<IUserService> _userService = new();
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _controller = new AuthController(_userService.Object);
    }

    [Fact]
    public async Task Authenticate_When_Success_Should_Return_Ok()
    {
        // Arrange
        var request = new UserLoginDto { Login = "admin", Password = "secret" };
        var response = new ServiceResponse<GetUserAuthenticatedDto>
        {
            Success = true,
            Data = new GetUserAuthenticatedDto { Name = "Admin" }
        };
        _userService.Setup(s => s.Login("admin", "secret")).ReturnsAsync(response);

        // Act
        var result = await _controller.Authenticate(request);

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        _userService.Verify(s => s.Login("admin", "secret"), Times.Once);
    }

    [Fact]
    public async Task Authenticate_When_Failure_Should_Return_Unauthorized()
    {
        // Arrange
        var request = new UserLoginDto { Login = "admin", Password = "wrong" };
        var response = new ServiceResponse<GetUserAuthenticatedDto>
        {
            Success = false,
            Message = "Invalid credentials"
        };
        _userService.Setup(s => s.Login("admin", "wrong")).ReturnsAsync(response);

        // Act
        var result = await _controller.Authenticate(request);

        // Assert
        result.Result.Should().BeOfType<UnauthorizedObjectResult>();
        _userService.Verify(s => s.Login("admin", "wrong"), Times.Once);
    }
}

