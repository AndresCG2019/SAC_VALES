using Microsoft.EntityFrameworkCore.Migrations;

namespace SAC_VALES.Web.Migrations
{
    public partial class changes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EmpresaVinculada",
                table: "Distribuidor",
                newName: "EmpresaVinculadaId");

            migrationBuilder.AlterColumn<string>(
                name: "EmpresaVinculadaId",
                table: "Distribuidor",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Distribuidor_EmpresaVinculadaId",
                table: "Distribuidor",
                column: "EmpresaVinculadaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Distribuidor_AspNetUsers_EmpresaVinculadaId",
                table: "Distribuidor",
                column: "EmpresaVinculadaId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Distribuidor_AspNetUsers_EmpresaVinculadaId",
                table: "Distribuidor");

            migrationBuilder.DropIndex(
                name: "IX_Distribuidor_EmpresaVinculadaId",
                table: "Distribuidor");

            migrationBuilder.RenameColumn(
                name: "EmpresaVinculadaId",
                table: "Distribuidor",
                newName: "EmpresaVinculada");

            migrationBuilder.AlterColumn<string>(
                name: "EmpresaVinculada",
                table: "Distribuidor",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
