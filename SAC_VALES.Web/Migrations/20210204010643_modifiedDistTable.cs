using Microsoft.EntityFrameworkCore.Migrations;

namespace SAC_VALES.Web.Migrations
{
    public partial class modifiedDistTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Apellidos",
                table: "Distribuidor",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Direccion",
                table: "Distribuidor",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nombre",
                table: "Distribuidor",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Telefono",
                table: "Distribuidor",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Apellidos",
                table: "Distribuidor");

            migrationBuilder.DropColumn(
                name: "Direccion",
                table: "Distribuidor");

            migrationBuilder.DropColumn(
                name: "Nombre",
                table: "Distribuidor");

            migrationBuilder.DropColumn(
                name: "Telefono",
                table: "Distribuidor");
        }
    }
}
