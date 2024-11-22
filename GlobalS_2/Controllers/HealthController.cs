using Microsoft.AspNetCore.Mvc;

namespace HealthController
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        // Rota para verificar o status do serviço
        [HttpGet("health")]
        public IActionResult GetHealthStatus()
        {
            return Ok(new { message = "Service is running" }); // Retorna 200 OK
        }
    }
}