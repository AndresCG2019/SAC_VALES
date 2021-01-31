using Microsoft.EntityFrameworkCore.Migrations;

namespace SAC_VALES.Web.Migrations
{
    public partial class addedAdminFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UsuarioId",
                table: "Administrador",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Administrador_UsuarioId",
                table: "Administrador",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Administrador_AspNetUsers_UsuarioId",
                table: "Administrador",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Administrador_AspNetUsers_UsuarioId",
                table: "Administrador");

            migrationBuilder.DropIndex(
                name: "IX_Administrador_UsuarioId",
                table: "Administrador");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Administrador");
        }
    }
}
