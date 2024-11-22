using GlobalS_2.Models;
using Microsoft.AspNetCore.Mvc;
using GlobalS_2.Repository;


namespace GlobalS_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsumoController : ControllerBase
    {
        private readonly IConsumoRepository _consumoRepository;

        public ConsumoController(IConsumoRepository consumoRepository)
        {
            _consumoRepository = consumoRepository;
        }

        // Rota para registrar um novo consumo
        [HttpPost("consumo")]
        public async Task<IActionResult> RegisterConsumo([FromBody] Consumo consumo)
        {
            if (consumo == null)
            {
                return BadRequest(new { message = "O corpo da requisição não pode ser nulo." });
            }

            if (consumo.Valor <= 0)
            {
                return BadRequest(new { message = "O valor de consumo deve ser maior que zero." });
            }

            if (string.IsNullOrEmpty(consumo.Data) || !DateTime.TryParse(consumo.Data, out _))
            {
                return BadRequest(new { message = "Data inválida. Por favor, forneça uma data válida." });
            }

            var savedConsumo = await _consumoRepository.AddConsumoAsync(consumo);
            return CreatedAtAction(nameof(GetConsumos), new { id = savedConsumo.Id }, savedConsumo);
        }

        // Rota para consultar todos os consumos registrados
        [HttpGet("consumo")]
        public async Task<IActionResult> GetConsumos([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page <= 0 || pageSize <= 0)
            {
                return BadRequest(new { message = "Parâmetros inválidos. A página e o tamanho da página devem ser maiores que zero." });
            }

            var skip = (page - 1) * pageSize;

            var consumos = await _consumoRepository.GetConsumosAsync(skip, pageSize);
            if (consumos == null || consumos.Count == 0)
            {
                return NotFound(new { message = "Nenhum consumo encontrado." });
            }

            return Ok(consumos);
        }
    }
}

