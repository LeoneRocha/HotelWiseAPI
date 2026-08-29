using HotelWise.API.Controllers;
using HotelWise.Core.SDK.Common;
using Microsoft.AspNetCore.Mvc;

namespace HotelWise.API.Tests.Controllers;

public class AppInformationVersionProductControllerTests
{
    private readonly AppInformationVersionProductController _controller = new();

    [Fact]
    public async Task GetString_Should_Return_Ok()
    {
        // Act
        var result = await _controller.GetString();

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var ok = (OkObjectResult)result.Result!;
        ok.Value.Should().BeOfType<string>().Which.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Get_Should_Return_Ok_With_Version_List()
    {
        // Act
        var result = await _controller.Get();

        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var ok = (OkObjectResult)result.Result!;
        var list = ok.Value.Should().BeAssignableTo<List<AppInformationVersionProductDto>>().Subject;
        list.Should().HaveCount(1);
    }
}
