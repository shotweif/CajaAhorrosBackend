using CajaAhorrosBackend.Models;
using Microsoft.AspNetCore.Mvc;

// namespace CajaAhorrosBackend.Contollers
// {
[ApiController]
[Route("api/[controller]")]
public class ClienteController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ClienteController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateCliente(Cliente cliente)
    {
        cliente.Password = GenerateMD5(cliente.Password);
        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();
        return Ok(new { message = "Cliente creado exitosamente." });
    }

    private string GenerateMD5(string input)
    {
        using var md5 = System.Security.Cryptography.MD5.Create();
        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
        byte[] hashBytes = md5.ComputeHash(inputBytes);
        return Convert.ToHexString(hashBytes);
    }
    [HttpGet("{id}")]
    public IActionResult GetCliente(int id)
    {
        // Lógica para obtener el cliente por ID
        return Ok(new { IdCliente = id, Nombre = "Cliente Ejemplo", CorreoElectronico = "cliente@ejemplo.com" });
    }

    // [HttpPost]
    // public IActionResult CreateCliente([FromBody] Cliente cliente)
    // {
    //     // Lógica para crear un cliente
    //     return CreatedAtAction(nameof(GetCliente), new { id = cliente.IdCliente }, cliente);
    // }

}

// }