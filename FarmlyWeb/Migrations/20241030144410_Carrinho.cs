using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarmlyWeb.Migrations
{
    /// <inheritdoc />
    public partial class Carrinho : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "quantidade_estoque",
                table: "tblProduto",
                newName: "quantidade");

            migrationBuilder.AlterColumn<decimal>(
                name: "preco_total",
                table: "tblVenda",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldPrecision: 18,
                oldScale: 2);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "quantidade",
                table: "tblProduto",
                newName: "quantidade_estoque");

            migrationBuilder.AlterColumn<int>(
                name: "preco_total",
                table: "tblVenda",
                type: "int",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);
        }
    }
}
