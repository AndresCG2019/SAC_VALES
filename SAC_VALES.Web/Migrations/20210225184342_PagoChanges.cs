using Microsoft.EntityFrameworkCore.Migrations;

namespace SAC_VALES.Web.Migrations
{
    public partial class PagoChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pago_ValeEntity_Valeid",
                table: "Pago");

            migrationBuilder.AlterColumn<int>(
                name: "Valeid",
                table: "Pago",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Pago_ValeEntity_Valeid",
                table: "Pago",
                column: "Valeid",
                principalTable: "ValeEntity",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pago_ValeEntity_Valeid",
                table: "Pago");

            migrationBuilder.AlterColumn<int>(
                name: "Valeid",
                table: "Pago",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Pago_ValeEntity_Valeid",
                table: "Pago",
                column: "Valeid",
                principalTable: "ValeEntity",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
