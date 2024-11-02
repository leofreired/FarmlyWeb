using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarmlyWeb.Migrations
{
    /// <inheritdoc />
    public partial class AddTabelaEstoque : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblEstoque",
                columns: table => new
                {
                    id_estoque = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_produto = table.Column<int>(type: "int", nullable: false),
                    quantidade = table.Column<int>(type: "int", nullable: false),
                    tipo_movimentacao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    data_movimentacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblEstoque", x => x.id_estoque);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblEstoque");
        }
    }
}
