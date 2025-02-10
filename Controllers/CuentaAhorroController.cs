using Microsoft.AspNetCore.Mvc;
using CajaAhorrosBackend.Models;
using CajaAhorrosBackend.Services;
using Microsoft.AspNetCore.Authorization;

namespace CajaAhorrosBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CuentaAhorroController : ControllerBase
    {
        private readonly AccountService _cuenta;


        public CuentaAhorroController(AccountService cuenta)
        {
            _cuenta = cuenta;
        }

        [HttpPost("CrearCuenta/{userId}")]
        public async Task<IActionResult> CrearCuenta(int userId)
        {
            try
            {
                if (userId == 0)
                {
                    return Unauthorized(new { message = "Usuario no autorizado." });
                }

                var result = await _cuenta.CreateNewAccount(userId);
                return result;
                // return Ok(new { success = true });

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error starting the proces: {ex.Message}");
                return StatusCode(500, new { Message = "An error occured while starting the process" });
            }

        }

        [HttpGet("ConsultarCuentas/{userId}")]
        public async Task<IActionResult> GetConsultarCuentas(int userId)
        {
            try
            {
                if (userId == 0)
                {
                    return Unauthorized(new { message = "Usuario no autorizado." });
                }

                var result = await _cuenta.ShowAccounts(userId);
                return result;
                // return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error starting the proces: {ex.Message}");
                return StatusCode(500, new { Message = "An error occured while starting the process" });
            }
        }

        [HttpGet("Validar/{accountNumber}")]
        public async Task<IActionResult> ValidatCuenta(string accountNumber)
        {
            Console.WriteLine(accountNumber);
            if (accountNumber.Length < 1)
            {
                return Unauthorized(new { message = "Numero de cuenta no valido." });
            }
            try
            {
                var result = await _cuenta.ValidateAccountNumber(accountNumber);
                return result;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error starting the proces: {ex.Message}");
                return StatusCode(500, new { Message = "An error occured while starting the process" });
            }
        }

        [HttpPut("IniciarTransferencia")]
        public async Task<IActionResult> IniciarTransferenciaAsync([FromBody] TransferenciaDto transferencia)
        {
            try
            {
                // if (!ModelState.IsValid)
                // {
                //     return BadRequest(ModelState);
                // }

                var result = await _cuenta.TransferirFondos(transferencia);
                return result;
                // return Ok(new { success = true, message = "Transferencia realizada exitosamente.", transferencia });

                // Procesar la transferencia
                // return Ok($"Transferencia realizada con éxito.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error starting the proces: {ex.Message}");
                return StatusCode(500, new { Message = "An error occured while starting the process" });
            }
        }

        [HttpGet("SummaryTransactions/{userId}")]
        public async Task<IActionResult> SummaryTransaction(int userId)
        {
            try
            {
                var result = await _cuenta.HistorialTransacciones(userId);
                return result;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error starting the proces: {ex.Message}");
                return StatusCode(500, new { Message = "An error occured while starting the process" });
            }
        }

        [HttpPost("SummaryTransactionsFilter/{userId}")]
        public async Task<IActionResult> SummaryTransactionFilter(int userId, [FromBody] TransactionFilterRequest filterData)
        {
            try
            {
                var result = await _cuenta.HistorialTransaccionesFilter(userId, filterData);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error processing SummaryTransactionFilter for user {UserId}", userId, ex);
                return StatusCode(500, new { Message = "An error occurred while processing the request." });
            }
        }


        [Authorize]
        [HttpGet]
        public IActionResult GetProtectedData()
        {
            return Ok(new { message = "Este es un recurso protegido" });
        }

    }

}