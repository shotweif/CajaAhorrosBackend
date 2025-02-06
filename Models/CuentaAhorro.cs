using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CajaAhorrosBackend.Models
{
    public class CuentaAhorro
    {
        [Key]
        public int IdCuenta { get; set; }

        public string NumeroCuenta { get; set; } = GenerateAccountNumber(); // Generar automáticamente

        public float Saldo { get; set; }

        public int IdUsuario { get; set; }

        public bool Activo { get; set; } // Nueva propiedad

        // Relacion
        [ForeignKey("IdUsuario")]
        public Cliente? Cliente { get; set; }
        
        // Método para generar un número de cuenta único
        private static string GenerateAccountNumber()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssfff"); // Alternativa: usar un generador aleatorio
        }
    }
}
