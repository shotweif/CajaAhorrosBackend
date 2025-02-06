using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CajaAhorrosBackend.Migrations
{
    /// <inheritdoc />
    public partial class RenameColumnsInTransaccion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TipoTransaccion",
                table: "Transacciones",
                newName: "IdCuentaOrige");

            migrationBuilder.RenameColumn(
                name: "IdCuenta",
                table: "Transacciones",
                newName: "IdCuentaDestino");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdCuentaOrige",
                table: "Transacciones",
                newName: "TipoTransaccion");

            migrationBuilder.RenameColumn(
                name: "IdCuentaDestino",
                table: "Transacciones",
                newName: "IdCuenta");
        }
    }
}
