using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiFuncionarios.Infra.Data.Migrations
{
    public partial class AddIdUsuario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "IdUsuario",
                table: "Funcionario",
                type: "uniqueidentifier",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdUsuario",
                table: "Funcionario");
        }
    }
}
