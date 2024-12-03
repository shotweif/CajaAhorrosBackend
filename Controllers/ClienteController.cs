using System;
using System.Text;
using Konscious.Security.Cryptography;
using CajaAhorrosBackend.Models;
using Microsoft.AspNetCore.Mvc;

// namespace CajaAhorrosBackend.Contollers
// {
[ApiController]
[Route("api/[controller]")]
public class ClienteController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ClienteController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateCliente(Cliente cliente)
    {
        cliente.Password = GenerateArgon2Hash(cliente.Password);
        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();
        return Ok(new { message = "Cliente creado exitosamente." });
    }

    // private string GenerateMD5(string input)
    // {
    //     using var md5 = System.Security.Cryptography.MD5.Create();
    //     byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
    //     byte[] hashBytes = md5.ComputeHash(inputBytes);
    //     return Convert.ToHexString(hashBytes);
    // }

    private string GenerateArgon2Hash(string input)
    {
        // Convertir la entrada en bytes
        byte[] salt = GenerateSalt(); // Debes generar un salt único para cada usuario
        byte[] inputBytes = Encoding.UTF8.GetBytes(input);

        using (var argon2 = new Argon2id(inputBytes))
        {
            argon2.Salt = salt;
            argon2.DegreeOfParallelism = 8; // Ajuste de acuerdo con los recursos de hardware
            argon2.MemorySize = 65536; // 64 MB
            argon2.Iterations = 4;     // Iteraciones para mayor seguridad

            byte[] hashBytes = argon2.GetBytes(32); // Tamaño del hash en bytes
            return Convert.ToBase64String(hashBytes);
        }
    }

    private byte[] GenerateSalt()
    {
        // Generar un salt aleatorio de 16 bytes
        using var rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
        byte[] salt = new byte[16];
        rng.GetBytes(salt);
        return salt;
    }
}

// }