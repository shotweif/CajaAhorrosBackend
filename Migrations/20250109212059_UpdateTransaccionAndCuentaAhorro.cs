using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CajaAhorrosBackend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTransaccionAndCuentaAhorro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Activa",
                table: "Transacciones");

            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "CuentasAhorro",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Activo",
                table: "CuentasAhorro");

            migrationBuilder.AddColumn<bool>(
                name: "Activa",
                table: "Transacciones",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
