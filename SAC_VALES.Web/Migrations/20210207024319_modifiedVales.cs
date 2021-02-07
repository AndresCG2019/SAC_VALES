using Microsoft.EntityFrameworkCore.Migrations;

namespace SAC_VALES.Web.Migrations
{
    public partial class modifiedVales : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ValeEntity_Cliente_Clienteid",
                table: "ValeEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_ValeEntity_Distribuidor_Distribuidorid",
                table: "ValeEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_ValeEntity_Empresa_Empresaid",
                table: "ValeEntity");

            migrationBuilder.DropIndex(
                name: "IX_ValeEntity_Clienteid",
                table: "ValeEntity");

            migrationBuilder.DropIndex(
                name: "IX_ValeEntity_Distribuidorid",
                table: "ValeEntity");

            migrationBuilder.DropIndex(
                name: "IX_ValeEntity_Empresaid",
                table: "ValeEntity");

            migrationBuilder.RenameColumn(
                name: "Empresaid",
                table: "ValeEntity",
                newName: "EmpresaId");

            migrationBuilder.RenameColumn(
                name: "Distribuidorid",
                table: "ValeEntity",
                newName: "DistribuidorId");

            migrationBuilder.RenameColumn(
                name: "Clienteid",
                table: "ValeEntity",
                newName: "ClienteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EmpresaId",
                table: "ValeEntity",
                newName: "Empresaid");

            migrationBuilder.RenameColumn(
                name: "DistribuidorId",
                table: "ValeEntity",
                newName: "Distribuidorid");

            migrationBuilder.RenameColumn(
                name: "ClienteId",
                table: "ValeEntity",
                newName: "Clienteid");

            migrationBuilder.CreateIndex(
                name: "IX_ValeEntity_Clienteid",
                table: "ValeEntity",
                column: "Clienteid");

            migrationBuilder.CreateIndex(
                name: "IX_ValeEntity_Distribuidorid",
                table: "ValeEntity",
                column: "Distribuidorid");

            migrationBuilder.CreateIndex(
                name: "IX_ValeEntity_Empresaid",
                table: "ValeEntity",
                column: "Empresaid");

            migrationBuilder.AddForeignKey(
                name: "FK_ValeEntity_Cliente_Clienteid",
                table: "ValeEntity",
                column: "Clienteid",
                principalTable: "Cliente",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ValeEntity_Distribuidor_Distribuidorid",
                table: "ValeEntity",
                column: "Distribuidorid",
                principalTable: "Distribuidor",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ValeEntity_Empresa_Empresaid",
                table: "ValeEntity",
                column: "Empresaid",
                principalTable: "Empresa",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
