using Microsoft.EntityFrameworkCore.Migrations;

namespace SAC_VALES.Web.Migrations
{
    public partial class addedEmpresaFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "representanteId",
                table: "Empresa",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Empresa_representanteId",
                table: "Empresa",
                column: "representanteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Empresa_AspNetUsers_representanteId",
                table: "Empresa",
                column: "representanteId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Empresa_AspNetUsers_representanteId",
                table: "Empresa");

            migrationBuilder.DropIndex(
                name: "IX_Empresa_representanteId",
                table: "Empresa");

            migrationBuilder.DropColumn(
                name: "representanteId",
                table: "Empresa");
        }
    }
}
