using CajaAhorrosBackend.Data;
using Microsoft.EntityFrameworkCore;
using CajaAhorrosBackend.Services;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Clave secreta para firmar el token
var keyVal = "ClaveSuper$ecreta123456789_ABCDEFGHIJKLMN";
var key = Encoding.UTF8.GetBytes(keyVal);


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "CajaAhorrosBackend",
            ValidAudience = "CajaAhorrosBackend",
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

builder.Services.AddAuthorization();

// Contexto de la base de datos
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddSingleton<PasswordService>();
builder.Services.AddScoped<ClienteService>();
builder.Services.AddSingleton(new TokenService(keyVal));
//Enable Cors
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();  // Habilita los endpoints para Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "CajaAhorros API",
        Description = "Documentación de la API para el proyecto CajaAhorrosBackend",
        Contact = new OpenApiContact
        {
            Name = "Equipo de Desarrollo",
            Email = "contacto@ejemplo.com",
            Url = new Uri("https://ejemplo.com")
        },
    });
});

var app = builder.Build();

// Enable Swagger and Swager UI
if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => {
        options.SwaggerEndpoint("/swagger/v1/swagger.json","Caja de Ahorros API v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseCors();

app.UseRouting();

// app.UseHttpsRedirection();

app.UseAuthentication(); // Habilitar autenticación
app.UseAuthorization();  // Habilitar autorización

app.MapControllers();

app.Run();
