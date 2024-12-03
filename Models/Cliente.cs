using System.ComponentModel.DataAnnotations;

namespace CajaAhorrosBackend.Models
{
    public class Cliente
    {
        [Key] // Esto marca 'IdCliente' como clave primaria
        public int IdCliente { get; set; }

        public string Nombre { get; set; }

        public string CorreoElectronico { get; set; }

        public string Password { get; set; }
    }
}
