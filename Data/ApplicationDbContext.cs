using CajaAhorrosBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace CajaAhorrosBackend.Data
{
    // dotnet ef migrations add InitialCreate
    // dotnet ef database update

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<CuentaAhorro> CuentasAhorro { get; set; }
        public DbSet<Transaccion> Transacciones { get; set; }
    }
}
