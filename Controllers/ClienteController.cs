using Microsoft.AspNetCore.Mvc;
using CajaAhorrosBackend.Models;
using CajaAhorrosBackend.Services;
using Microsoft.AspNetCore.Authorization;

namespace CajaAhorrosBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly ClienteService _cliente;

        public ClientesController(ClienteService client)
        {
            _cliente = client;
        }

        [HttpPost("Register")] // Registrar usuario
        public async Task<IActionResult> Register([FromBody] Cliente cliente)
        {
            try
            {
                if (cliente == null || string.IsNullOrEmpty(cliente.Nombre))
                {
                    return BadRequest(new { Message = "Invalid data." });
                }

                Console.WriteLine($"Se va a crear el usuario {cliente.CorreoElectronico}");
                var result = await _cliente.CreateClientAsync(cliente);

                return result;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error starting the proces: {ex.Message}");
                return StatusCode(500, new { Message = "An error occured while starting the process" });
            }
        }

        [HttpPost("Login")] // Iniciar sesion 
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                if (request == null || string.IsNullOrEmpty(request.CorreoElectronico) || string.IsNullOrEmpty(request.Password))
                {
                    return BadRequest(new { Message = "Invalid data." });
                }

                var result = await _cliente.ValidateClientAsync(request);
                Console.WriteLine($"Se valido el usuario {request.CorreoElectronico}");

                return result;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error starting the proces: {ex.Message}");
                return StatusCode(500, new { Message = "An error occured while starting the process" });
            }
        }

        // [Authorize]
        [HttpGet("Perfil/{userWebId}")] // Trae la cuenta del usuario
        public async Task<IActionResult> GetPerfilUsuario(string userWebId)
        {
            try
            {
                if (string.IsNullOrEmpty(userWebId))
                {
                    return BadRequest(new { message = "Usuario no autorizado." });
                }

                var result = await _cliente.UserProfile(userWebId);
                Console.WriteLine($"data retornada: {result}");

                return result;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error starting the proces: {ex.Message}");
                return StatusCode(500, new { Message = "An error occured while starting the process" });
            }
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetProtectedData()
        {
            return Ok(new { message = "Este es un recurso protegido" });
        }
    }
}