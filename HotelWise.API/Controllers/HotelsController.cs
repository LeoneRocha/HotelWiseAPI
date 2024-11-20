using HotelWise.Domain.Dto;
using HotelWise.Domain.Helpers;
using HotelWise.Domain.Interfaces.Entity;
using HotelWise.Domain.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelWise.API.Controllers
{
    [Authorize("Bearer")]
    [ApiController]
    [Route("api/[controller]/v1")]
    public class HotelsController : ControllerBase
    {
        private readonly IHotelService _hotelService;
        public HotelsController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        private void setUserIdCurrent()
        {
            _hotelService.SetUserId(GetUserIdCurrent());
        }

        private long GetUserIdCurrent()
        {
            long idUser = SecurityHelperApi.GetUserIdApi(User);
            return idUser;

        }
        /// <summary>
        /// Obtém todos os hotéis.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var hotels = await _hotelService.GetAllHotelsAsync();
            return Ok(hotels);
        }
        /// <summary>
        /// Obtém um hotel pelo ID.
        /// </summary>
        /// <param name="id">ID do hotel</param>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(long id)
        {
            var hotel = await _hotelService.GetHotelByIdAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }
            return Ok(hotel);
        }

        [HttpGet("addvector/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddVectorById(long id)
        {
            var result = await _hotelService.InsertHotelInVectorStore(id);
            return Ok(result);
        }

        [HttpGet("generate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Generate()
        {
            var hotel = await _hotelService.GenerateHotelByIA();
            return Ok(hotel);
        }
        [HttpPost("semanticsearch")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> SemanticSearch([FromBody] SearchCriteria searchCriteria)
        {
            var hotels = await _hotelService.SemanticSearch(searchCriteria);
            return Ok(hotels);
        }
        /// <summary>
        /// Cria um novo hotel.
        /// </summary>
        /// <param name="hotel">Dados do hotel</param>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] HotelDto hotel)
        {
            setUserIdCurrent();
            var response = await _hotelService.AddHotelAsync(hotel);

            return Ok(response);
        }
        /// <summary>
        /// Atualiza um hotel existente.
        /// </summary>
        /// <param name="id">ID do hotel</param>
        /// <param name="hotel">Dados atualizados do hotel</param>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(long id, [FromBody] HotelDto hotel)
        {
            if (id != hotel.HotelId)
            {
                return BadRequest();
            }
            setUserIdCurrent();
            var response = await _hotelService.UpdateHotelAsync(hotel);
            return Ok(response);
        }
        /// <summary>
        /// Deleta um hotel pelo ID.
        /// </summary>
        /// <param name="id">ID do hotel</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(long id)
        {
            var response = await _hotelService.DeleteHotelAsync(id);
            return Ok(response);
        }
    }
}
