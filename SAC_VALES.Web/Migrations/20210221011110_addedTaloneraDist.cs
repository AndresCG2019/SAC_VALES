using Microsoft.EntityFrameworkCore.Migrations;

namespace SAC_VALES.Web.Migrations
{
    public partial class addedTaloneraDist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Distribuidorid",
                table: "Talonera",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Talonera_Distribuidorid",
                table: "Talonera",
                column: "Distribuidorid");

            migrationBuilder.AddForeignKey(
                name: "FK_Talonera_Distribuidor_Distribuidorid",
                table: "Talonera",
                column: "Distribuidorid",
                principalTable: "Distribuidor",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Talonera_Distribuidor_Distribuidorid",
                table: "Talonera");

            migrationBuilder.DropIndex(
                name: "IX_Talonera_Distribuidorid",
                table: "Talonera");

            migrationBuilder.DropColumn(
                name: "Distribuidorid",
                table: "Talonera");
        }
    }
}
