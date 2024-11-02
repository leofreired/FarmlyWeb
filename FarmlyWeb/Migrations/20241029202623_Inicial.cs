using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarmlyWeb.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblCliente",
                columns: table => new
                {
                    id_cliente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cpf = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    senha = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    endereco = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    telefone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCliente", x => x.id_cliente);
                });

            migrationBuilder.CreateTable(
                name: "tblProduto",
                columns: table => new
                {
                    id_produto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Preco = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    quantidade_estoque = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblProduto", x => x.id_produto);
                });

            migrationBuilder.CreateTable(
                name: "tblVenda",
                columns: table => new
                {
                    id_venda = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_cliente = table.Column<int>(type: "int", nullable: false),
                    data_venda = table.Column<DateTime>(type: "datetime2", nullable: false),
                    preco_total = table.Column<int>(type: "int", precision: 18, scale: 2, nullable: false),
                    forma_pagamento = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblVenda", x => x.id_venda);
                });

            migrationBuilder.CreateTable(
                name: "tblVendaItens",
                columns: table => new
                {
                    id_item = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_venda = table.Column<int>(type: "int", nullable: false),
                    id_produto = table.Column<int>(type: "int", nullable: false),
                    quantidade = table.Column<int>(type: "int", nullable: false),
                    preco_unitario = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblVendaItens", x => x.id_item);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblCliente");

            migrationBuilder.DropTable(
                name: "tblProduto");

            migrationBuilder.DropTable(
                name: "tblVenda");

            migrationBuilder.DropTable(
                name: "tblVendaItens");
        }
    }
}
