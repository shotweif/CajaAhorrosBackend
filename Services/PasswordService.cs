using Konscious.Security.Cryptography;
using System.Text;

namespace CajaAhorrosBackend.Services
{
    public class PasswordService
    {
        public string HashPassword(string password)
        {
            using (var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password)))
            {
                argon2.Salt = Encoding.UTF8.GetBytes("TuClaveDeSalSegura");
                argon2.DegreeOfParallelism = 8; // Número de hilos
                argon2.MemorySize = 65536; // Memoria en KB
                argon2.Iterations = 4; // Iteraciones

                return Convert.ToBase64String(argon2.GetBytes(32)); // Hash de 32 bytes
            }
        }

        public bool VerifyPassword(string hashedPassword, string inputPassword)
        {
            return HashPassword(inputPassword) == hashedPassword;
        }
    }
}
