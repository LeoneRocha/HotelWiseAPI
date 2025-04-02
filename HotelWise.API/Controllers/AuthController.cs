using HotelWise.Domain.Dto;
using HotelWise.Domain.Dto.Enitty;
using HotelWise.Domain.Interfaces.Entity;
using Microsoft.AspNetCore.Mvc;

namespace HotelWise.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/v1")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        public AuthController(IUserService userService)
        {
            _userService = userService;
        } 

        [HttpPost("Authenticate")]
        public async Task<ActionResult<ServiceResponse<GetUserAuthenticatedDto>>> Authenticate(UserLoginDto request)
        {
            var response = await _userService.Login(request.Login, request.Password);
            if (!response.Success)
            {
                return Unauthorized(response);
            }
            return Ok(response);
        } 
    }
}
