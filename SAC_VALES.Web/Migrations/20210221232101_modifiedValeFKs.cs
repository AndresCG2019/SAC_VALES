using Microsoft.EntityFrameworkCore.Migrations;

namespace SAC_VALES.Web.Migrations
{
    public partial class modifiedValeFKs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<int>(
                name: "Empresaid",
                table: "ValeEntity",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "Distribuidorid",
                table: "ValeEntity",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "Clienteid",
                table: "ValeEntity",
                nullable: true,
                oldClrType: typeof(int));

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
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ValeEntity_Distribuidor_Distribuidorid",
                table: "ValeEntity",
                column: "Distribuidorid",
                principalTable: "Distribuidor",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ValeEntity_Empresa_Empresaid",
                table: "ValeEntity",
                column: "Empresaid",
                principalTable: "Empresa",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<int>(
                name: "EmpresaId",
                table: "ValeEntity",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DistribuidorId",
                table: "ValeEntity",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ClienteId",
                table: "ValeEntity",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
