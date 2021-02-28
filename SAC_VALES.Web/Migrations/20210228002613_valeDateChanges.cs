using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SAC_VALES.Web.Migrations
{
    public partial class valeDateChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Año",
                table: "ValeEntity");

            migrationBuilder.DropColumn(
                name: "Mes",
                table: "ValeEntity");

            migrationBuilder.DropColumn(
                name: "dia",
                table: "ValeEntity");

            migrationBuilder.RenameColumn(
                name: "Fecha",
                table: "ValeEntity",
                newName: "FechaPrimerPago");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "ValeEntity",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "ValeEntity");

            migrationBuilder.RenameColumn(
                name: "FechaPrimerPago",
                table: "ValeEntity",
                newName: "Fecha");

            migrationBuilder.AddColumn<int>(
                name: "Año",
                table: "ValeEntity",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Mes",
                table: "ValeEntity",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "dia",
                table: "ValeEntity",
                nullable: false,
                defaultValue: 0);
        }
    }
}
