using Microsoft.EntityFrameworkCore.Migrations;

namespace SAC_VALES.Web.Migrations
{
    public partial class DistEntityPagos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Distribuidorid",
                table: "Pago",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pago_Distribuidorid",
                table: "Pago",
                column: "Distribuidorid");

            migrationBuilder.AddForeignKey(
                name: "FK_Pago_Distribuidor_Distribuidorid",
                table: "Pago",
                column: "Distribuidorid",
                principalTable: "Distribuidor",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pago_Distribuidor_Distribuidorid",
                table: "Pago");

            migrationBuilder.DropIndex(
                name: "IX_Pago_Distribuidorid",
                table: "Pago");

            migrationBuilder.DropColumn(
                name: "Distribuidorid",
                table: "Pago");
        }
    }
}
