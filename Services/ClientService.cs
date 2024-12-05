using System.Text;
using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using CajaAhorrosBackend.Models;
using Microsoft.AspNetCore.Mvc;

namespace CajaAhorrosBackend.Services
{
    public class ClienteService : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ClienteService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OkObjectResult> CreateClientAsync(Cliente cliente)
        {
            cliente.Password = GenerateArgon2Hash(cliente.Password);
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Cliente creado exitosamente." });
        }
        private string GenerateArgon2Hash(string input)
        {
            Console.WriteLine("Entramos a crear");

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
            Console.WriteLine("Entramos a crear");

            // Generar un salt aleatorio de 16 bytes
            return RandomNumberGenerator.GetBytes(16);
        }
        
        // Creador implicito temp
        public static implicit operator ClienteService(Cliente v)
        {
            throw new NotImplementedException();
        }
    }
}