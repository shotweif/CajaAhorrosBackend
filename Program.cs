using CajaAhorrosBackend.Data;
using Microsoft.EntityFrameworkCore;
using CajaAhorrosBackend.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Agregar el contexto de la base de datos
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddSingleton<PasswordService>();
builder.Services.AddScoped<ClienteService>();

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

// builder.Services.AddEndpointsApiExplorer();  // Habilita los endpoints para Swagger
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
if(app.Environment.IsDevelopment()){
    app.UseSwagger();
    app.UseSwaggerUI(options => {
        options.SwaggerEndpoint("/swagger/v1/swagger.json","Caja de Ahorros API v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseCors();

app.UseRouting();

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
