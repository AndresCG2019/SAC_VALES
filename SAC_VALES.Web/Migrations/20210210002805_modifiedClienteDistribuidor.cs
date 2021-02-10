using Microsoft.EntityFrameworkCore.Migrations;

namespace SAC_VALES.Web.Migrations
{
    public partial class modifiedClienteDistribuidor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cliente_AspNetUsers_DistribuidorId",
                table: "Cliente");

            migrationBuilder.RenameColumn(
                name: "DistribuidorId",
                table: "Cliente",
                newName: "Distribuidorid");

            migrationBuilder.RenameIndex(
                name: "IX_Cliente_DistribuidorId",
                table: "Cliente",
                newName: "IX_Cliente_Distribuidorid");

            migrationBuilder.AlterColumn<int>(
                name: "Distribuidorid",
                table: "Cliente",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Cliente_Distribuidor_Distribuidorid",
                table: "Cliente",
                column: "Distribuidorid",
                principalTable: "Distribuidor",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cliente_Distribuidor_Distribuidorid",
                table: "Cliente");

            migrationBuilder.RenameColumn(
                name: "Distribuidorid",
                table: "Cliente",
                newName: "DistribuidorId");

            migrationBuilder.RenameIndex(
                name: "IX_Cliente_Distribuidorid",
                table: "Cliente",
                newName: "IX_Cliente_DistribuidorId");

            migrationBuilder.AlterColumn<string>(
                name: "DistribuidorId",
                table: "Cliente",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Cliente_AspNetUsers_DistribuidorId",
                table: "Cliente",
                column: "DistribuidorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
