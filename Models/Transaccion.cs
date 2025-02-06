using System.ComponentModel.DataAnnotations;

namespace CajaAhorrosBackend.Models
{
    public class Transaccion
    {
        [Key]
        public int IdTransaccion { get; set; }

        public string IdCuentaOrigen { get; set; }

        public string IdCuentaDestino { get; set; }

        public float Monto { get; set; }

        public DateTime FechaTransaccion { get; set; }
    }
}