using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FarmlyWeb.Migrations
{
    /// <inheritdoc />
    public partial class FormaPagamentoString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "forma_pagamento",
                table: "tblVenda",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "string",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "forma_pagamento",
                table: "tblVenda",
                type: "string",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
