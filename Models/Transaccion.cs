using System.ComponentModel.DataAnnotations;

namespace CajaAhorrosBackend.Models
{
    public class Transaccion
    {
        [Key]
        public int IdTransaccion { get; set; }

        public int IdCuenta { get; set; }

        public int TipoTransaccion { get; set; }

        public float Monto { get; set; }

        public DateTime FechaTransaccion { get; set; }
    }
}