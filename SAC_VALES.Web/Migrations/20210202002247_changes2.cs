using Microsoft.EntityFrameworkCore.Migrations;

namespace SAC_VALES.Web.Migrations
{
    public partial class changes2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Distribuidor_AspNetUsers_UsuarioId",
                table: "Distribuidor");

            migrationBuilder.RenameColumn(
                name: "UsuarioId",
                table: "Distribuidor",
                newName: "UsuarioVinculadoId");

            migrationBuilder.RenameIndex(
                name: "IX_Distribuidor_UsuarioId",
                table: "Distribuidor",
                newName: "IX_Distribuidor_UsuarioVinculadoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Distribuidor_AspNetUsers_UsuarioVinculadoId",
                table: "Distribuidor",
                column: "UsuarioVinculadoId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Distribuidor_AspNetUsers_UsuarioVinculadoId",
                table: "Distribuidor");

            migrationBuilder.RenameColumn(
                name: "UsuarioVinculadoId",
                table: "Distribuidor",
                newName: "UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_Distribuidor_UsuarioVinculadoId",
                table: "Distribuidor",
                newName: "IX_Distribuidor_UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Distribuidor_AspNetUsers_UsuarioId",
                table: "Distribuidor",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
