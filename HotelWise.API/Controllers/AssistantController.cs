using HotelWise.Domain.Dto;
using HotelWise.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HotelWise.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/v1")]
    public class AssistantController : ControllerBase
    {
        private readonly IAssistantService _assistantService;
        public AssistantController(IAssistantService assistantService)
        {
            _assistantService = assistantService;
        }

        [HttpPost("ask")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> AskAssistant([FromBody] SearchCriteria searchCriteria)
        {
            var result = await _assistantService.AskAssistant(searchCriteria);
            return Ok(result);
        }
    }
}