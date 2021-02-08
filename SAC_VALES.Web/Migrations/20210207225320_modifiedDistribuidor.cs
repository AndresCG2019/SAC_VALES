using Microsoft.EntityFrameworkCore.Migrations;

namespace SAC_VALES.Web.Migrations
{
    public partial class modifiedDistribuidor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Distribuidor_AspNetUsers_EmpresaVinculadaId",
                table: "Distribuidor");

            migrationBuilder.RenameColumn(
                name: "EmpresaVinculadaId",
                table: "Distribuidor",
                newName: "EmpresaVinculadaid");

            migrationBuilder.RenameIndex(
                name: "IX_Distribuidor_EmpresaVinculadaId",
                table: "Distribuidor",
                newName: "IX_Distribuidor_EmpresaVinculadaid");

            migrationBuilder.AlterColumn<int>(
                name: "EmpresaVinculadaid",
                table: "Distribuidor",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Distribuidor_Empresa_EmpresaVinculadaid",
                table: "Distribuidor",
                column: "EmpresaVinculadaid",
                principalTable: "Empresa",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Distribuidor_Empresa_EmpresaVinculadaid",
                table: "Distribuidor");

            migrationBuilder.RenameColumn(
                name: "EmpresaVinculadaid",
                table: "Distribuidor",
                newName: "EmpresaVinculadaId");

            migrationBuilder.RenameIndex(
                name: "IX_Distribuidor_EmpresaVinculadaid",
                table: "Distribuidor",
                newName: "IX_Distribuidor_EmpresaVinculadaId");

            migrationBuilder.AlterColumn<string>(
                name: "EmpresaVinculadaId",
                table: "Distribuidor",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Distribuidor_AspNetUsers_EmpresaVinculadaId",
                table: "Distribuidor",
                column: "EmpresaVinculadaId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
