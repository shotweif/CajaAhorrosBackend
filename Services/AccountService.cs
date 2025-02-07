using CajaAhorrosBackend.Models;
using Microsoft.AspNetCore.Mvc;
using CajaAhorrosBackend.Data;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
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
            try
            {
                var cuentaExiste = await _context.CuentasAhorro.Where(c => c.Activo == true).FirstOrDefaultAsync(c => c.NumeroCuenta == accountNumber);

                if (cuentaExiste != null)
                {
                    var DuenioCuenta = await _context.Clientes.FirstOrDefaultAsync(c => c.IdCliente == cuentaExiste.IdUsuario);
                    return Ok(new { success = true, dataRes = DuenioCuenta?.Apellido + " " + DuenioCuenta?.Nombre });
                }

                return Ok(new { success = false, message = "La cuenta no existe o esta temporalmente desabilidata." });

            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: ", ex);
                return Ok(new { message = "Error al crear el cliente: ", ex });
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

            await _context.SaveChangesAsync();

            var FechaMovimiento = DateTime.UtcNow;
            var newTransferencia = new Transaccion
            {
                IdCuentaOrigen = transferencia.IdCuentaOrigen,
                IdCuentaDestino = transferencia.IdCuentaDestino,
                Monto = transferencia.Monto,
                FechaTransaccion = FechaMovimiento
            };

            _context.Transacciones.Add(newTransferencia);
            await _context.SaveChangesAsync();
            var newTransferenciaId = newTransferencia.IdTransaccion;

            return Ok(new { success = true, message = "Transferencia realizada exitosamente.", dataRes = FechaMovimiento, newTransferenciaId });
        }

        public async Task<IActionResult> HistorialTransacciones(int userId)
        {
            if (userId < 1)
            {
                return Ok(new { success = false, message = "El ID de usuario debe ser mayor a 0." });
            }

            var cuentasDelUsuario = await _context.CuentasAhorro
                .Where(c => c.IdUsuario == userId)
                .Select(c => new
                {
                    IdCuenta = c.IdCuenta,
                    NumeroCuenta = c.NumeroCuenta
                })
                .ToListAsync();

            if (!cuentasDelUsuario.Any())
            {
                return Ok(new { success = false, message = "No se encontraron cuentas para el ID de usuario especificado." });
            }

            var historial = await _context.Transacciones
                .Where(t => cuentasDelUsuario.Select(c => c.NumeroCuenta).Contains(t.IdCuentaOrigen)
                || cuentasDelUsuario.Select(c => c.NumeroCuenta).Contains(t.IdCuentaDestino))
                .ToListAsync();

            return Ok(new { success = true, cuentasDelUsuario, historial });
        }

        public async Task<IActionResult> HistorialTransaccionesFilter(int userId, TransactionFilterRequest filterData)
        {
            if (userId < 1)
            {
                return Ok(new { success = false, message = "El ID de usuario debe ser mayor a 0." });
            }

            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.IdCliente == userId);


            if (cliente == null)
            {
                return Ok(new { success = false, message = "No se encontraron cuentas para el usuario de usuario especificado." });
            }

            var DateSelect = DateTime.ParseExact(filterData.DateFilter, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToUniversalTime();
            var historial = await _context.Transacciones
                            .Where(t => 
                            t.FechaTransaccion.ToUniversalTime().Date == DateSelect.Date &
                            (t.IdCuentaOrigen == filterData.AccountFilter | t.IdCuentaDestino == filterData.AccountFilter)
                             )
                            .ToListAsync();

            return Ok(new { success = true, historial });
        }




    }

}