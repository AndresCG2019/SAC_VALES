using Microsoft.EntityFrameworkCore.Migrations;

namespace SAC_VALES.Web.Migrations
{
    public partial class modifiedClienteTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Empresa",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Distribuidor",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Apellidos",
                table: "Cliente",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Direccion",
                table: "Cliente",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Cliente",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nombre",
                table: "Cliente",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Telefono",
                table: "Cliente",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Administrador",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Empresa");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Distribuidor");

            migrationBuilder.DropColumn(
                name: "Apellidos",
                table: "Cliente");

            migrationBuilder.DropColumn(
                name: "Direccion",
                table: "Cliente");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Cliente");

            migrationBuilder.DropColumn(
                name: "Nombre",
                table: "Cliente");

            migrationBuilder.DropColumn(
                name: "Telefono",
                table: "Cliente");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Administrador");
        }
    }
}
