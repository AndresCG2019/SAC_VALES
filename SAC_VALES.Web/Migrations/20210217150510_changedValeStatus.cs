using Microsoft.EntityFrameworkCore.Migrations;

namespace SAC_VALES.Web.Migrations
{
    public partial class changedValeStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "status_vale",
                table: "ValeEntity",
                nullable: true,
                defaultValue: "Activo",
                oldClrType: typeof(bool));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "status_vale",
                table: "ValeEntity",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
