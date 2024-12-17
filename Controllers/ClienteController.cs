using Microsoft.AspNetCore.Mvc;
using CajaAhorrosBackend.Models;
using CajaAhorrosBackend.Services;

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

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] Cliente cliente)
        {
            try
            {
                if (cliente == null || string.IsNullOrEmpty(cliente.Nombre))
                {
                    return BadRequest(new { Message = "Invalid data." });
                }

                var result = await _cliente.CreateClientAsync(cliente);
                Console.WriteLine($"Se va a crear el usuario {cliente.CorreoElectronico}");

                return result;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error starting the proces: {ex.Message}");
                return StatusCode(500, new { Message = "An error occured while starting the process" });
            }
        }

        [HttpPost("Login")]
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


        // private readonly ClienteService _cliente;
        // private readonly ApplicationDbContext _context;

        // public ClienteController(ClienteService cliente, ApplicationDbContext context)
        // {
        //     _cliente = cliente;
        //     // _context = context;
        // }

        // [HttpPost("create")]
        // public async Task<IActionResult> StartCreateUser([FromBody] Cliente cliente)
        // {
        //     try
        //     {
        //         if (cliente == null || string.IsNullOrEmpty(cliente.Nombre))
        //         {
        //             return BadRequest(new { Message = "Invalid data." });
        //         }
        //         var result = _cliente.CreateClient(cliente);

        //         return result;

        //         // return Ok(new { Message = "Process started." });
        //     }
        //     catch (Exception ex)
        //     {
        //         Console.Error.WriteLine($"Error starting the proces: {ex.Message}");
        //         return StatusCode(500, new { Message = "An error occured while starting the process" });
        //     }
        // }







        // public IActionResult CreateCliente([FromBody] Cliente cliente)
        // {
        // Lógica para crear el cliente
        // return CreatedAtAction(nameof(CreateCliente), new { id = cliente.IdCliente }, cliente);
        // }

        // [HttpPost("create")]
        // public async Task<IActionResult> CreateCliente(Cliente cliente)
        // {
        //     Console.WriteLine("Entramos a crear");
        //     cliente.Password = GenerateArgon2Hash(cliente.Password);
        //     _context.Clientes.Add(cliente);
        //     await _context.SaveChangesAsync();
        //     return Ok(new { message = "Cliente creado exitosamente." });
        // }

        // private string GenerateMD5(string input)
        // {
        //     using var md5 = System.Security.Cryptography.MD5.Create();
        //     byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
        //     byte[] hashBytes = md5.ComputeHash(inputBytes);
        //     return Convert.ToHexString(hashBytes);
        // }

        // private string GenerateArgon2Hash(string input)
        // {
        //     Console.WriteLine("Entramos a crear");

        //     // Convertir la entrada en bytes
        //     byte[] salt = GenerateSalt(); // Debes generar un salt único para cada usuario
        //     byte[] inputBytes = Encoding.UTF8.GetBytes(input);

        //     using (var argon2 = new Argon2id(inputBytes))
        //     {
        //         argon2.Salt = salt;
        //         argon2.DegreeOfParallelism = 8; // Ajuste de acuerdo con los recursos de hardware
        //         argon2.MemorySize = 65536; // 64 MB
        //         argon2.Iterations = 4;     // Iteraciones para mayor seguridad

        //         byte[] hashBytes = argon2.GetBytes(32); // Tamaño del hash en bytes
        //         return Convert.ToBase64String(hashBytes);
        //     }
        // }

        // private byte[] GenerateSalt()
        // {
        //     Console.WriteLine("Entramos a crear");

        //     // Generar un salt aleatorio de 16 bytes
        //     return RandomNumberGenerator.GetBytes(16);
        // }
    }

}