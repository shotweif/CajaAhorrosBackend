using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configuración del DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
                     new MySqlServerVersion(new Version(8, 0, 32))));


// Configurar servicios
builder.Services.AddControllers();  // Agrega los controladores al contenedor de servicios
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
// Habilitar Swagger y Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "CajaAhorros API v1");
        options.RoutePrefix = string.Empty; // Hace que Swagger UI esté en la raíz
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
