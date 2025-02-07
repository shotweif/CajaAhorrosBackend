using CajaAhorrosBackend.Data;
using Microsoft.EntityFrameworkCore;
using CajaAhorrosBackend.Services;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Athorization
var keyVal = "ClaveSuper$ecreta123456789_ABCDEFGHIJKLMN";
var key = Encoding.UTF8.GetBytes(keyVal);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "CajaAhorrosBackend",
            ValidAudience = "CajaAhorrosBackend",
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };

        // Añade esto para depuración
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine("OnAuthenticationFailed: " + context.Exception.Message);

                if (context.Exception is SecurityTokenInvalidSignatureException)
                {
                    Console.WriteLine("La firma del token es inválida.");
                }
                else if (context.Exception is SecurityTokenExpiredException)
                {
                    Console.WriteLine("El token ha expirado.");
                }
                else if (context.Exception is SecurityTokenInvalidAudienceException)
                {
                    Console.WriteLine("El público (audience) del token no es válido.");
                }
                else if (context.Exception is SecurityTokenInvalidIssuerException)
                {
                    Console.WriteLine("El emisor (issuer) del token no es válido.");
                }

                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine("OnTokenValidated: " + context.SecurityToken);
                return Task.CompletedTask;
            },
            OnMessageReceived = context =>
            {
                if (string.IsNullOrEmpty(context.Token))
                {
                    // Busca el token en un parámetro de consulta
                    var token = context.Request.Query["access_token"];
                    if (!string.IsNullOrEmpty(token))
                    {
                        context.Token = token;
                    }
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddSingleton(new TokenService(key));

// Contexto de la base de datos
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddSingleton<PasswordService>();
builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<AccountService>();

// Habilitar Cors
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

// Habilita los endpoints para Swagger
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen(options =>
// {
//     options.SwaggerDoc("v1", new OpenApiInfo
//     {
//         Version = "v1",
//         Title = "CajaAhorros API",
//         Description = "Documentación de la API para el proyecto CajaAhorrosBackend",
//         Contact = new OpenApiContact
//         {
//             Name = "Equipo de Desarrollo",
//             Email = "contacto@ejemplo.com",
//             Url = new Uri("https://ejemplo.com")
//         },
//     });
// });

var app = builder.Build();

// Enable Swagger and Swagger UI
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI(options =>
//     {
//         options.SwaggerEndpoint("/swagger/v1/swagger.json", "Caja de Ahorros API v1");
//         options.RoutePrefix = string.Empty;
//     });
// }

// app.UseCors();
app.UseCors(builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyHeader()
           .AllowAnyMethod();
});


// app.Use(async (context, next) =>
// {
//     var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
//     Console.WriteLine($"Authorization Header: {authHeader}");

//     if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
//     {
//         var token = authHeader.Substring("Bearer ".Length).Trim();
//         Console.WriteLine($"Token recibido: {token}");
//     }
//     else
//     {
//         Console.WriteLine("No se recibió un token o está en formato incorrecto.");
//     }

//     await next();
// });

app.UseRouting();

app.UseAuthentication(); // Habilitar autenticación
app.UseAuthorization();  // Habilitar autorización

app.MapControllers();

app.Run();
