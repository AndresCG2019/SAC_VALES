using Microsoft.EntityFrameworkCore.Migrations;

namespace SAC_VALES.Web.Migrations
{
    public partial class modifiedForeignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Administrador_AspNetUsers_UsuarioId",
                table: "Administrador");

            migrationBuilder.DropForeignKey(
                name: "FK_Cliente_AspNetUsers_ClienteId",
                table: "Cliente");

            migrationBuilder.DropForeignKey(
                name: "FK_Distribuidor_AspNetUsers_UsuarioVinculadoId",
                table: "Distribuidor");

            migrationBuilder.DropForeignKey(
                name: "FK_Empresa_AspNetUsers_representanteId",
                table: "Empresa");

            migrationBuilder.RenameColumn(
                name: "representanteId",
                table: "Empresa",
                newName: "EmpresaAuthId");

            migrationBuilder.RenameIndex(
                name: "IX_Empresa_representanteId",
                table: "Empresa",
                newName: "IX_Empresa_EmpresaAuthId");

            migrationBuilder.RenameColumn(
                name: "UsuarioVinculadoId",
                table: "Distribuidor",
                newName: "DistribuidorAuthId");

            migrationBuilder.RenameIndex(
                name: "IX_Distribuidor_UsuarioVinculadoId",
                table: "Distribuidor",
                newName: "IX_Distribuidor_DistribuidorAuthId");

            migrationBuilder.RenameColumn(
                name: "ClienteId",
                table: "Cliente",
                newName: "ClienteAuthId");

            migrationBuilder.RenameIndex(
                name: "IX_Cliente_ClienteId",
                table: "Cliente",
                newName: "IX_Cliente_ClienteAuthId");

            migrationBuilder.RenameColumn(
                name: "UsuarioId",
                table: "Administrador",
                newName: "AdminAuthId");

            migrationBuilder.RenameIndex(
                name: "IX_Administrador_UsuarioId",
                table: "Administrador",
                newName: "IX_Administrador_AdminAuthId");

            migrationBuilder.AddForeignKey(
                name: "FK_Administrador_AspNetUsers_AdminAuthId",
                table: "Administrador",
                column: "AdminAuthId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Cliente_AspNetUsers_ClienteAuthId",
                table: "Cliente",
                column: "ClienteAuthId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Distribuidor_AspNetUsers_DistribuidorAuthId",
                table: "Distribuidor",
                column: "DistribuidorAuthId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Empresa_AspNetUsers_EmpresaAuthId",
                table: "Empresa",
                column: "EmpresaAuthId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Administrador_AspNetUsers_AdminAuthId",
                table: "Administrador");

            migrationBuilder.DropForeignKey(
                name: "FK_Cliente_AspNetUsers_ClienteAuthId",
                table: "Cliente");

            migrationBuilder.DropForeignKey(
                name: "FK_Distribuidor_AspNetUsers_DistribuidorAuthId",
                table: "Distribuidor");

            migrationBuilder.DropForeignKey(
                name: "FK_Empresa_AspNetUsers_EmpresaAuthId",
                table: "Empresa");

            migrationBuilder.RenameColumn(
                name: "EmpresaAuthId",
                table: "Empresa",
                newName: "representanteId");

            migrationBuilder.RenameIndex(
                name: "IX_Empresa_EmpresaAuthId",
                table: "Empresa",
                newName: "IX_Empresa_representanteId");

            migrationBuilder.RenameColumn(
                name: "DistribuidorAuthId",
                table: "Distribuidor",
                newName: "UsuarioVinculadoId");

            migrationBuilder.RenameIndex(
                name: "IX_Distribuidor_DistribuidorAuthId",
                table: "Distribuidor",
                newName: "IX_Distribuidor_UsuarioVinculadoId");

            migrationBuilder.RenameColumn(
                name: "ClienteAuthId",
                table: "Cliente",
                newName: "ClienteId");

            migrationBuilder.RenameIndex(
                name: "IX_Cliente_ClienteAuthId",
                table: "Cliente",
                newName: "IX_Cliente_ClienteId");

            migrationBuilder.RenameColumn(
                name: "AdminAuthId",
                table: "Administrador",
                newName: "UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_Administrador_AdminAuthId",
                table: "Administrador",
                newName: "IX_Administrador_UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Administrador_AspNetUsers_UsuarioId",
                table: "Administrador",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Cliente_AspNetUsers_ClienteId",
                table: "Cliente",
                column: "ClienteId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Distribuidor_AspNetUsers_UsuarioVinculadoId",
                table: "Distribuidor",
                column: "UsuarioVinculadoId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Empresa_AspNetUsers_representanteId",
                table: "Empresa",
                column: "representanteId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
