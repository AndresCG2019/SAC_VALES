using Microsoft.EntityFrameworkCore.Migrations;

namespace SAC_VALES.Web.Migrations
{
    public partial class modifiedEmpresa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NombreEmpresa",
                table: "Empresa",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 6);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NombreEmpresa",
                table: "Empresa",
                maxLength: 6,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
