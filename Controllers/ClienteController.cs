using System;
using System.Text;
using Konscious.Security.Cryptography;
using CajaAhorrosBackend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using CajaAhorrosBackend.Services;

// namespace CajaAhorrosBackend.Controllers
// {
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        // private readonly ClienteService _clientService;
        // // private readonly ApplicationDbContext _context;  

        // public ClienteController(Cliente cliente)
        // {
        //     Console.WriteLine(cliente);
        //     _clientService = cliente;
        // //     _context = context;
        // }

        [HttpPost("create")]
        public IActionResult SatrtCreateUser([FromBody] Cliente cliente)
        {
            Console.WriteLine("Se accesio.");
            return Ok(new {Message = "Coneccion corecta."});
        }
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

// }