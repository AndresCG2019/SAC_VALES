using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SAC_VALES.Web.Migrations
{
    public partial class addedVales2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ValeEntity",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Monto = table.Column<float>(nullable: false),
                    Distribuidorid = table.Column<int>(nullable: true),
                    Empresaid = table.Column<int>(nullable: true),
                    Clienteid = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValeEntity", x => x.id);
                    table.ForeignKey(
                        name: "FK_ValeEntity_Cliente_Clienteid",
                        column: x => x.Clienteid,
                        principalTable: "Cliente",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ValeEntity_Distribuidor_Distribuidorid",
                        column: x => x.Distribuidorid,
                        principalTable: "Distribuidor",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ValeEntity_Empresa_Empresaid",
                        column: x => x.Empresaid,
                        principalTable: "Empresa",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ValeEntity");
        }
    }
}
