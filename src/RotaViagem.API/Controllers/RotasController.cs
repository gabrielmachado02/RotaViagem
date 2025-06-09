using Microsoft.AspNetCore.Mvc;
using RotaViagem.Application.DTOs;
using RotaViagem.Application.Services;

namespace RotaViagem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class RotasController : ControllerBase
    {
        private readonly IRotaService _rotaService;

        public RotasController(IRotaService rotaService)
        {
            _rotaService = rotaService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<RotaDto>>> GetAll()
        {
            var result = await _rotaService.GetAllRotasAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RotaDto>> GetById(Guid id)
        {
            var result = await _rotaService.GetRotaByIdAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RotaDto>> Create([FromBody] RotaInputDto rotaDto)
        {
            var result = await _rotaService.CreateRotaAsync(rotaDto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid id, [FromBody] RotaInputDto rotaDto)
        {
            await _rotaService.UpdateRotaAsync(id, rotaDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _rotaService.DeleteRotaAsync(id);
            return NoContent();
        }

        [HttpGet("consulta")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<MelhorRotaResultadoDto>> ConsultarMelhorRota([FromQuery] string origem, [FromQuery] string destino)
        {
            var consultaDto = new ConsultaRotaDto
            {
                Origem = origem.ToUpper(),
                Destino = destino.ToUpper()
            };

            var result = await _rotaService.ConsultarMelhorRotaAsync(consultaDto);
            
            if (result.Caminho.Count == 0)
            {
                return NotFound(new { message = $"Não foi possível encontrar uma rota de {origem} para {destino}" });
            }
            
            return Ok(result);
        }

        [HttpGet("consulta/{rota}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<string>> ConsultarMelhorRotaTexto(string rota)
        {
            var partes = rota.Split('-');
            if (partes.Length != 2)
            {
                return BadRequest(new { message = "Formato inválido. Use ORIGEM-DESTINO (Ex: GRU-CDG)" });
            }

            var consultaDto = new ConsultaRotaDto
            {
                Origem = partes[0].ToUpper(),
                Destino = partes[1].ToUpper()
            };

            var result = await _rotaService.ConsultarMelhorRotaAsync(consultaDto);
            
            if (result.Caminho.Count == 0)
            {
                return NotFound(new { message = $"Não foi possível encontrar uma rota de {partes[0]} para {partes[1]}" });
            }
            
            return Ok(result.ResultadoCompleto);
        }
    }
}