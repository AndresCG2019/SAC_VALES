using Microsoft.EntityFrameworkCore.Migrations;

namespace SAC_VALES.Web.Migrations
{
    public partial class taloneraRange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumeroFolio",
                table: "ValeEntity",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Taloneraid",
                table: "ValeEntity",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ValeEntity_Taloneraid",
                table: "ValeEntity",
                column: "Taloneraid");

            migrationBuilder.AddForeignKey(
                name: "FK_ValeEntity_Talonera_Taloneraid",
                table: "ValeEntity",
                column: "Taloneraid",
                principalTable: "Talonera",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ValeEntity_Talonera_Taloneraid",
                table: "ValeEntity");

            migrationBuilder.DropIndex(
                name: "IX_ValeEntity_Taloneraid",
                table: "ValeEntity");

            migrationBuilder.DropColumn(
                name: "NumeroFolio",
                table: "ValeEntity");

            migrationBuilder.DropColumn(
                name: "Taloneraid",
                table: "ValeEntity");
        }
    }
}
