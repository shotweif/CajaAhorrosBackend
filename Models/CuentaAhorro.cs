using System.ComponentModel.DataAnnotations;

namespace CajaAhorrosBackend.Models
{
    public class CuentaAhorro
    {
        [Key]
        public int IdCuenta { get; set; }

        public float Saldo { get; set; }

        public int IdUsuario { get; set; }
    }
}
