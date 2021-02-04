using Microsoft.EntityFrameworkCore.Migrations;

namespace SAC_VALES.Web.Migrations
{
    public partial class modifiedEmpresaTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApellidosRepresentante",
                table: "Empresa",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NombreRepresentante",
                table: "Empresa",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TelefonoRepresentante",
                table: "Empresa",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApellidosRepresentante",
                table: "Empresa");

            migrationBuilder.DropColumn(
                name: "NombreRepresentante",
                table: "Empresa");

            migrationBuilder.DropColumn(
                name: "TelefonoRepresentante",
                table: "Empresa");
        }
    }
}
