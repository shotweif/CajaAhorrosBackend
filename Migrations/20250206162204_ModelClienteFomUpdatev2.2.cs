using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CajaAhorrosBackend.Migrations
{
    /// <inheritdoc />
    public partial class ModelClienteFomUpdatev22 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Apellido",
                table: "Clientes",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Apellido",
                table: "Clientes");
        }
    }
}
