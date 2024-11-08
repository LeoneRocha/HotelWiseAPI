using HotelWise.Domain.Dto;
using HotelWise.Domain.Interfaces;
using HotelWise.Domain.Model;
using Microsoft.AspNetCore.Mvc;

namespace HotelWise.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/v1")]
    public class HotelsController : ControllerBase
    {
        private readonly IHotelService _hotelService;

        public HotelsController(IHotelService hotelService)
        {
            _hotelService = hotelService;
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


        [HttpGet("generate/{numberGerate}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Generate(int numberGerate)
        {
            var hotels = await _hotelService.GenerateHotelsByIA(numberGerate);
            return Ok(hotels); 
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
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] Hotel hotel)
        {
            await _hotelService.AddHotelAsync(hotel);
            return CreatedAtAction(nameof(GetById), new { id = hotel.HotelId }, hotel);
        }

        /// <summary>
        /// Atualiza um hotel existente.
        /// </summary>
        /// <param name="id">ID do hotel</param>
        /// <param name="hotel">Dados atualizados do hotel</param>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(long id, [FromBody] Hotel hotel)
        {
            if (id != hotel.HotelId)
            {
                return BadRequest();
            }
            await _hotelService.UpdateHotelAsync(hotel);
            return NoContent();
        }

        /// <summary>
        /// Deleta um hotel pelo ID.
        /// </summary>
        /// <param name="id">ID do hotel</param>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(long id)
        {
            await _hotelService.DeleteHotelAsync(id);
            return NoContent();
        }
    } 
}
