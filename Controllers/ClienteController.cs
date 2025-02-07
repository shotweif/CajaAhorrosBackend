using Microsoft.AspNetCore.Mvc;
using CajaAhorrosBackend.Models;
using CajaAhorrosBackend.Services;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using System.Security.Claims;
using System.Text;

namespace CajaAhorrosBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly ClienteService _cliente;
        private readonly AccountService _cuenta;

        public ClientesController(ClienteService client, AccountService account)
        {
            _cliente = client;
            _cuenta = account;
        }

        [HttpPost("Register")] // Registrar usuario
        public async Task<IActionResult> Register([FromBody] Cliente cliente)
        {
            try
            {
                if (cliente == null || string.IsNullOrEmpty(cliente.Nombre))
                {
                    return BadRequest(new { Message = "Invalid data." });
                }

                Console.WriteLine($"Se va a crear el usuario {cliente.CorreoElectronico}");
                var result = await _cliente.CreateClientAsync(cliente);

                return result;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error starting the proces: {ex.Message}");
                return StatusCode(500, new { Message = "An error occured while starting the process" });
            }
        }

        [HttpPost("Login")] // Iniciar sesion 
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                if (request == null || string.IsNullOrEmpty(request.CorreoElectronico) || string.IsNullOrEmpty(request.Password))
                {
                    return BadRequest(new { Message = "Invalid data." });
                }

                var result = await _cliente.ValidateClientAsync(request);
                Console.WriteLine($"Se valido el usuario {request.CorreoElectronico}");

                return result;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error starting the proces: {ex.Message}");
                return StatusCode(500, new { Message = "An error occured while starting the process" });
            }
        }

        // [Authorize]
        [HttpGet("Perfil/{userWebId}")] // Trae la cuenta del usuario
        public async Task<IActionResult> GetPerfilUsuario(string userWebId)
        {
            try
            {
                if (string.IsNullOrEmpty(userWebId))
                {
                    return BadRequest(new { message = "Usuario no autorizado." });
                }

                var result = await _cliente.UserProfile(userWebId);
                Console.WriteLine($"data retornada: {result}");

                return result;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error starting the proces: {ex.Message}");
                return StatusCode(500, new { Message = "An error occured while starting the process" });
            }
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

        [HttpPost("IniciarTransferencia")]
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

        // [HttpPost("IniciarTranferencia")]
        // public async Task<IActionResult> IniciarTransferencia([FromBody] string IdCuentaOrigen)
        // {
        //     {
        //         try
        //         {
        //             Console.WriteLine($"\nDesde: {IdCuentaOrigen}\nHacia: \nMonto: ");
        //             // return Ok(new { success = true, message = "Transferencia realizada exitosamente." });

        //             var result = await _cuenta.TransferirFondos(IdCuentaOrigen );
        //             return result;
        //         }
        //         catch (Exception ex)
        //         {
        //             Console.Error.WriteLine($"Error starting the proces: {ex.Message}");
        //             return StatusCode(500, new { Message = "An error occured while starting the process" });
        //         }
        //     }
        // }

        // [HttpPost("IniciarTranferencia")]
        // public IActionResult Transfair([FromBody] string IdCuentaOrigen, string IdCuentaDestino, float Monto)
        // {
        //     Console.WriteLine($"\nDesde: {IdCuentaOrigen}\nHacia: {IdCuentaDestino}\nMonto: {Monto}");
        //     return Ok(new { success = true, message = "Transferencia realizada exitosamente." });
        //     // try
        //     // {
        //     //     // Console.WriteLine($"SE DEBE ESTAR REALIZANDO\n");

        //     //     var result = await _cuenta.TransferirFondos(IdCuentaOrigen, IdCuentaDestino, Monto);
        //     //     return result;
        //     // }
        //     // catch (Exception ex)
        //     // {
        //     //     Console.Error.WriteLine($"Error starting the proces: {ex.Message}");
        //     //     return StatusCode(500, new { Message = "An error occured while starting the process" });
        //     // }
        // }

        [Authorize]
        [HttpGet]
        public IActionResult GetProtectedData()
        {
            return Ok(new { message = "Este es un recurso protegido" });
        }

    }
}