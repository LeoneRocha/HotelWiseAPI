using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelWise.API.Tests.Controllers;

/// <summary>
/// Helpers compartilhados para testes de controllers (claims de usuário autenticado).
/// </summary>
internal static class ControllerTestHelper
{
    public static void SetAuthenticatedUser(ControllerBase controller, long userId = 1)
    {
        var identity = new ClaimsIdentity(
        [
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        ], authenticationType: "Test");

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(identity)
            }
        };
    }
}
