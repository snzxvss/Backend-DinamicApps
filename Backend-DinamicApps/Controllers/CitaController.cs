using Models.Request;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace Backend_DinamicApps.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CitaController : ControllerBase
    {
        private readonly ICitaService _citaService;
        public CitaController(ICitaService citaService)
        {
            _citaService = citaService;
        }

        [HttpGet("disponibles")]
        public async Task<IActionResult> Disponibles([FromQuery] string especialidad)
        {
            var result = await _citaService.ObtenerCitasDisponiblesAsync(especialidad);
            return Ok(result);
        }

        [HttpPost("reservar")]
        public async Task<IActionResult> Reservar([FromBody] ReservarCitaRequest request)
        {
            var result = await _citaService.ReservarCitaAsync(request.CitaId, request.PacienteId);
            if (result.Message.Contains("correctamente"))
                return Ok(result);
            return BadRequest(result);
        }
    }
}
