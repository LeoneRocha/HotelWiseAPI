using HotelWise.Domain.Dto;
using HotelWise.Domain.Helpers;
using HotelWise.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelWise.API.Controllers
{
    [Authorize("Bearer")]
    [ApiController]
    [Route("api/[controller]/v1")]
    public class AssistantController : ControllerBase
    {
        private readonly IAssistantService _assistantService;
        public AssistantController(IAssistantService assistantService)
        {
            _assistantService = assistantService;
        }
        private void setUserIdCurrent()
        {
            _assistantService.SetUserId(GetUserIdCurrent());
        }
        private long GetUserIdCurrent()
        {
            long idUser = SecurityHelperApi.GetUserIdApi(User);
            return idUser;
        }

        [HttpPost("ask")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AskAssistant([FromBody] AskAssistantRequest request)
        {
            setUserIdCurrent();
            var result = await _assistantService.AskAssistant(request);
            if (result != null && result.Length > 0)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}