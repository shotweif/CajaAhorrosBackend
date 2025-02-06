using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CajaAhorrosBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddClientAccountRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Rol",
                table: "Clientes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "IX_CuentasAhorro_IdUsuario",
                table: "CuentasAhorro",
                column: "IdUsuario");

            migrationBuilder.AddForeignKey(
                name: "FK_CuentasAhorro_Clientes_IdUsuario",
                table: "CuentasAhorro",
                column: "IdUsuario",
                principalTable: "Clientes",
                principalColumn: "IdCliente",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CuentasAhorro_Clientes_IdUsuario",
                table: "CuentasAhorro");

            migrationBuilder.DropIndex(
                name: "IX_CuentasAhorro_IdUsuario",
                table: "CuentasAhorro");

            migrationBuilder.AlterColumn<string>(
                name: "Rol",
                table: "Clientes",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
