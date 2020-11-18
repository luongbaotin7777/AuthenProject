using Microsoft.EntityFrameworkCore.Migrations;

namespace AuthenProject.Migrations
{
    public partial class crete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_AppRoleClaims",
                table: "AppRoleClaims");

            migrationBuilder.RenameTable(
                name: "AppRoleClaims",
                newName: "RoleClaims");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoleClaims",
                table: "RoleClaims",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RoleClaims",
                table: "RoleClaims");

            migrationBuilder.RenameTable(
                name: "RoleClaims",
                newName: "AppRoleClaims");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppRoleClaims",
                table: "AppRoleClaims",
                column: "Id");
        }
    }
}
