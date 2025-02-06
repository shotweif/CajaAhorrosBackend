using CajaAhorrosBackend.Models;
using Microsoft.AspNetCore.Mvc;
using CajaAhorrosBackend.Data;
using Microsoft.EntityFrameworkCore;
// using System.Security.Claims;
using System.Text;

namespace CajaAhorrosBackend.Services
{
    public class ClienteService : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordService _passwordService;
        private readonly TokenService _tokenService;

        public ClienteService(ApplicationDbContext context, PasswordService passwordService, TokenService tokenService)
        {
            _context = context;
            _passwordService = passwordService;
            _tokenService = tokenService;
        }

        public async Task<IActionResult> CreateClientAsync(Cliente cliente) // SIGNUP
        {
            try
            {
                var emailExists = await _context.Clientes.AnyAsync(c => c.CorreoElectronico == cliente.CorreoElectronico);
                if (emailExists)
                {
                    return BadRequest(new { message = $"The mail {cliente.CorreoElectronico} already exists." });
                }

                cliente.Rol = "Cliente";
                cliente.Password = _passwordService.HashPassword(cliente.Password);

                _context.Clientes.Add(cliente);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Client created successfully.", cliente });
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: ", ex);
                return Ok(new { message = "Error al crear el cliente: ", ex });
            }
        }

        public async Task<IActionResult> ValidateClientAsync(LoginRequest request) // LOGIN
        {
            Console.WriteLine("se va a iniciar");
            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.CorreoElectronico == request.CorreoElectronico);
            if (cliente == null || !_passwordService.VerifyPassword(cliente.Password, request.Password))
            {
                return Unauthorized(new { success = false, message = "Correo o contraseña incorrectos", comparacion = $"el correo {cliente.CorreoElectronico}" });
            }

            cliente.UltimaFechaLogin = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            string userWebId = Convert.ToBase64String(Encoding.UTF8.GetBytes(cliente.CorreoElectronico));
            var token = _tokenService.GenerateToken(cliente.IdCliente.ToString(), cliente.CorreoElectronico);

            return Ok(new { success = true, token, userWebId });
        }

        public async Task<IActionResult> UserProfile(string userWebId)
        {
            // Buscar el usuario en la base de datos
            string correoDecodificado = Encoding.UTF8.GetString(Convert.FromBase64String(userWebId));
            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.CorreoElectronico == correoDecodificado);
            if (cliente == null)
            {
                return NotFound(new { message = "Usuario no encontrado." });
            }

            // Retornar el perfil del usuario
            return Ok(new
                {
                    cliente.IdCliente,
                    cliente.CorreoElectronico,
                    cliente.Nombre,
                    cliente.Phone,
                    cliente.Rol,
                    cliente.UltimaFechaLogin
                });
        }
    }
}