// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.DependencyInjection;
// using CajaAhorrosBackend.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Configuración del DbContext
// builder.Services.AddDbContext<ApplicationDbContext>(options =>
//     options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
//                      new MySqlServerVersion(new Version(8, 0, 32))));

// builder.Services.AddSingleton<ClienteService>();

// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

// builder.Services.AddCors(options =>
// {
//     options.AddDefaultPolicy(policy =>
//     {
//         policy.AllowAnyOrigin()
//         .AllowAnyHeader()
//         .AllowAnyMethod();
//     });
// });

var app = builder.Build();

// app.UseCors();
// app.UseRouting();

// app.UseAuthorization();
app.MapControllers();

app.Run();
