using Microsoft.EntityFrameworkCore.Migrations;

namespace SAC_VALES.Web.Migrations
{
    public partial class AtributosDeFechaEnVales : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
