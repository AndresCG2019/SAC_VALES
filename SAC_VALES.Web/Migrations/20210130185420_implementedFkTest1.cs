using Microsoft.EntityFrameworkCore.Migrations;

namespace SAC_VALES.Web.Migrations
{
    public partial class implementedFkTest1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DistribuidorEntity_AspNetUsers_UsuarioEntityId",
                table: "DistribuidorEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DistribuidorEntity",
                table: "DistribuidorEntity");

            migrationBuilder.RenameTable(
                name: "DistribuidorEntity",
                newName: "Distribuidor");

            migrationBuilder.RenameColumn(
                name: "UsuarioEntityId",
                table: "Distribuidor",
                newName: "UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_DistribuidorEntity_UsuarioEntityId",
                table: "Distribuidor",
                newName: "IX_Distribuidor_UsuarioId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Distribuidor",
                table: "Distribuidor",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Distribuidor_AspNetUsers_UsuarioId",
                table: "Distribuidor",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Distribuidor_AspNetUsers_UsuarioId",
                table: "Distribuidor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Distribuidor",
                table: "Distribuidor");

            migrationBuilder.RenameTable(
                name: "Distribuidor",
                newName: "DistribuidorEntity");

            migrationBuilder.RenameColumn(
                name: "UsuarioId",
                table: "DistribuidorEntity",
                newName: "UsuarioEntityId");

            migrationBuilder.RenameIndex(
                name: "IX_Distribuidor_UsuarioId",
                table: "DistribuidorEntity",
                newName: "IX_DistribuidorEntity_UsuarioEntityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DistribuidorEntity",
                table: "DistribuidorEntity",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_DistribuidorEntity_AspNetUsers_UsuarioEntityId",
                table: "DistribuidorEntity",
                column: "UsuarioEntityId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
