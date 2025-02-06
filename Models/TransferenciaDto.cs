using System.ComponentModel.DataAnnotations;

namespace CajaAhorrosBackend.Models
{
    public class TransferenciaDto
{
    [Required]
    public string IdCuentaOrigen { get; set; }

    [Required]
    public string IdCuentaDestino { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor que cero.")]
    public float Monto { get; set; }
}
}