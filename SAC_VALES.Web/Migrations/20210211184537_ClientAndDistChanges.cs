using Microsoft.EntityFrameworkCore.Migrations;

namespace SAC_VALES.Web.Migrations
{
    public partial class ClientAndDistChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cliente_Distribuidor_Distribuidorid",
                table: "Cliente");

            migrationBuilder.DropForeignKey(
                name: "FK_Distribuidor_Empresa_EmpresaVinculadaid",
                table: "Distribuidor");

            migrationBuilder.DropIndex(
                name: "IX_Distribuidor_EmpresaVinculadaid",
                table: "Distribuidor");

            migrationBuilder.DropIndex(
                name: "IX_Cliente_Distribuidorid",
                table: "Cliente");

            migrationBuilder.DropColumn(
                name: "EmpresaVinculadaid",
                table: "Distribuidor");

            migrationBuilder.DropColumn(
                name: "Distribuidorid",
                table: "Cliente");

            migrationBuilder.CreateTable(
                name: "ClienteDistribuidor",
                columns: table => new
                {
                    ClienteId = table.Column<int>(nullable: false),
                    DistribuidorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClienteDistribuidor", x => new { x.ClienteId, x.DistribuidorId });
                    table.ForeignKey(
                        name: "FK_ClienteDistribuidor_Cliente_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Cliente",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClienteDistribuidor_Distribuidor_DistribuidorId",
                        column: x => x.DistribuidorId,
                        principalTable: "Distribuidor",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClienteDistribuidor_DistribuidorId",
                table: "ClienteDistribuidor",
                column: "DistribuidorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClienteDistribuidor");

            migrationBuilder.AddColumn<int>(
                name: "EmpresaVinculadaid",
                table: "Distribuidor",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Distribuidorid",
                table: "Cliente",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Distribuidor_EmpresaVinculadaid",
                table: "Distribuidor",
                column: "EmpresaVinculadaid");

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_Distribuidorid",
                table: "Cliente",
                column: "Distribuidorid");

            migrationBuilder.AddForeignKey(
                name: "FK_Cliente_Distribuidor_Distribuidorid",
                table: "Cliente",
                column: "Distribuidorid",
                principalTable: "Distribuidor",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Distribuidor_Empresa_EmpresaVinculadaid",
                table: "Distribuidor",
                column: "EmpresaVinculadaid",
                principalTable: "Empresa",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
