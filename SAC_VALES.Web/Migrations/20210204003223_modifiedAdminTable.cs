using Microsoft.EntityFrameworkCore.Migrations;

namespace SAC_VALES.Web.Migrations
{
    public partial class modifiedAdminTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApellidoM",
                table: "Administrador");

            migrationBuilder.DropColumn(
                name: "ApellidoP",
                table: "Administrador");

            migrationBuilder.AddColumn<string>(
                name: "Apellidos",
                table: "Administrador",
                maxLength: 90,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Apellidos",
                table: "Administrador");

            migrationBuilder.AddColumn<string>(
                name: "ApellidoM",
                table: "Administrador",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ApellidoP",
                table: "Administrador",
                maxLength: 30,
                nullable: false,
                defaultValue: "");
        }
    }
}
