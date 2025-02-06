using CajaAhorrosBackend.Models;
using Microsoft.AspNetCore.Mvc;
using CajaAhorrosBackend.Data;
using Microsoft.EntityFrameworkCore;
// using System.Security.Claims;
// using System.Text;

namespace CajaAhorrosBackend.Services
{
    public class AccountService : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AccountService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> CreateNewAccount(int userWebId)
        {
            // Generar un número de cuenta único
            string numeroCuenta;
            do
            {
                var random = new Random();
                numeroCuenta = $"{random.Next(100000, 999999)}{random.Next(100000, 999999)}{random.Next(1000, 9999)}";
            } while (await _context.CuentasAhorro.AnyAsync(c => c.NumeroCuenta == numeroCuenta));

            // Crear una nueva cuenta asociada al usuario
            var nuevaCuenta = new CuentaAhorro
            {
                IdUsuario = userWebId,
                NumeroCuenta = numeroCuenta,
                Saldo = 0
            };

            _context.CuentasAhorro.Add(nuevaCuenta);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "Cuenta creada exitosamente.",
            });
        }

        public async Task<IActionResult> ShowAccounts(int userId)
        {
            // var accounts = await _context.CuentasAhorro.Where(c => c.IdUsuario == userId).ToListAsync();
            // Console.WriteLine($"LAS CIENTAS SON: {accounts}");
            var cuentas = await _context.CuentasAhorro
                    .Where(c => c.IdUsuario == userId)
                    .Select(c => new
                    {
                        c.IdCuenta,
                        c.NumeroCuenta,
                        c.Saldo,
                        c.Activo
                    })
                    .ToListAsync();
            if (cuentas == null || cuentas.Count == 0)
            {
                return NotFound(new { message = "No se han abierto cuentas", cuentas });
            }

            return Ok(new { cuentas });
        }

        public async Task<IActionResult> ValidateAccountNumber(string accountNumber)
        {
            var cuentaExiste = await _context.CuentasAhorro.AnyAsync(c => c.NumeroCuenta == accountNumber);
            if (cuentaExiste)
            {
                return Ok(new { success = true });
            }
            else
            {
                return Ok(new { success = false });
            }
        }
        public async Task<IActionResult> TransferirFondos(TransferenciaDto transferencia)
        {
            var cuentaOrigen = await _context.CuentasAhorro.FirstOrDefaultAsync(c => c.NumeroCuenta == transferencia.IdCuentaOrigen);
            var cuentaDestino = await _context.CuentasAhorro.FirstOrDefaultAsync(c => c.NumeroCuenta == transferencia.IdCuentaDestino);

            if (cuentaOrigen == null)
            {
                return NotFound(new { success = false, message = "Cuenta de origen no encontrada." });
            }

            if (cuentaDestino == null)
            {
                return NotFound(new { success = false, message = "Cuenta de destino no encontrada." });
            }

            if (cuentaOrigen.Saldo < transferencia.Monto)
            {
                return BadRequest(new { success = false, message = "Saldo insuficiente en la cuenta de origen." });
            }

            if (!cuentaDestino.Activo)
            {
                return BadRequest(new { success = false, message = "La cuenta de destino no está activa." });
            }

            cuentaOrigen.Saldo -= transferencia.Monto;
            cuentaDestino.Saldo += transferencia.Monto;

            var newTransferencia = new Transaccion
            {
                IdCuentaOrigen = transferencia.IdCuentaOrigen,
                IdCuentaDestino = transferencia.IdCuentaDestino,
                Monto = transferencia.Monto,
                FechaTransaccion = DateTime.Now
            };
            
            _context.Transacciones.Add(newTransferencia);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Transferencia realizada exitosamente." });
        }
    }

}