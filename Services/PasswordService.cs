using Konscious.Security.Cryptography;
using System.Text;

namespace CajaAhorrosBackend.Services
{
    public class PasswordService
    {
        public string HashPassword(string password)
        {
            using (var hasher = new Argon2id(Encoding.UTF8.GetBytes(password)))
            {
                hasher.Salt = Encoding.UTF8.GetBytes("unique_salt");
                hasher.DegreeOfParallelism = 8; // Número de threads
                hasher.MemorySize = 65536;     // Tamaño de memoria en KB
                hasher.Iterations = 4;        // Número de iteraciones

                return Convert.ToBase64String(hasher.GetBytes(32));
            }
        }

        public bool VerifyPassword(string password, string hash)
        {
            var hashedInput = HashPassword(password);
            return hashedInput == hash;
        }
    }
}
