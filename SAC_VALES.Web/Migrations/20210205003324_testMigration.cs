using Microsoft.EntityFrameworkCore.Migrations;

namespace SAC_VALES.Web.Migrations
{
    public partial class testMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NombreEmpresa",
                table: "Empresa",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NombreEmpresa",
                table: "Empresa",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);
        }
    }
}
