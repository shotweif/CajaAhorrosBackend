using System.ComponentModel.DataAnnotations;

namespace CajaAhorrosBackend.Models
{
    public class Cliente
    {
        [Key] // Esto marca 'IdCliente' como clave primaria
        public int IdCliente { get; set; }

        public string? Nombre { get; set; }
        
        public string? Apellido { get; set; }

        public string? Phone { get; set; }

        public required string CorreoElectronico { get; set; }

        public required string Password { get; set; }
        
        public DateTime? UltimaFechaLogin { get; set; }

        public string? Rol { get; set; }
    }
    
    public class LoginRequest
    {
        public required string CorreoElectronico { get; set; }

        public required string Password { get; set; }

    }
}
