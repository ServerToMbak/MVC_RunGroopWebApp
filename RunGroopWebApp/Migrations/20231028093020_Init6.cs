using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RunGroopWebApp.Migrations
{
    public partial class Init6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Races_AspNetUsers_UserId",
                table: "Races");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Races",
                newName: "AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Races_UserId",
                table: "Races",
                newName: "IX_Races_AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Races_AspNetUsers_AppUserId",
                table: "Races",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Races_AspNetUsers_AppUserId",
                table: "Races");

            migrationBuilder.RenameColumn(
                name: "AppUserId",
                table: "Races",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Races_AppUserId",
                table: "Races",
                newName: "IX_Races_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Races_AspNetUsers_UserId",
                table: "Races",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
