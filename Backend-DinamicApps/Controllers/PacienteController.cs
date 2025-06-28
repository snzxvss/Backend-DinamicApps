using Models.Request;
using Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Backend_DinamicApps.Helpers;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace Backend_DinamicApps.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PacienteController : ControllerBase
    {
        private readonly IPacienteService _pacienteService;
        private readonly IConfiguration _configuration;
        public PacienteController(IPacienteService pacienteService, IConfiguration configuration)
        {
            _pacienteService = pacienteService;
            _configuration = configuration;
        }

        [HttpPost("autenticar")]
        public async Task<IActionResult> Autenticar([FromBody] AutenticarPacienteRequest request)
        {
            var result = await _pacienteService.AutenticarPacienteAsync(request.Documento, request.FechaNacimiento);
            if (result.Content.Any())
            {
                var paciente = result.Content.First();
                var secret = _configuration["Jwt:Secret"];
                var token = JwtHelper.GenerateJwtToken(paciente.Id.ToString(), paciente.Nombres, secret);
                // Crear un nuevo objeto paciente con el token incluido
                var pacienteConToken = new {
                    paciente.Id,
                    paciente.Nombres,
                    paciente.Apellidos,
                    paciente.Documento,
                    paciente.FechaNacimiento,
                    paciente.Telefono,
                    token
                };
                return Ok(new {
                    content = pacienteConToken,
                    error = result.Error,
                    message = result.Message,
                    statusCode = result.StatusCode
                });
            }
            return NotFound(result);
        }
    }
}
