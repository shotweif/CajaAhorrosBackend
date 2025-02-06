using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CajaAhorrosBackend.Migrations
{
    /// <inheritdoc />
    public partial class Activa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Activa",
                table: "Transacciones",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Activa",
                table: "Transacciones");
        }
    }
}
