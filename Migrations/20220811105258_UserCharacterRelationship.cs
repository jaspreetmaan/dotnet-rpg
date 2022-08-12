using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dotnet_rpg.Migrations
{
    public partial class UserCharacterRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "userid",
                table: "Characters",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Characters_userid",
                table: "Characters",
                column: "userid");

            migrationBuilder.AddForeignKey(
                name: "FK_Characters_Users_userid",
                table: "Characters",
                column: "userid",
                principalTable: "Users",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Characters_Users_userid",
                table: "Characters");

            migrationBuilder.DropIndex(
                name: "IX_Characters_userid",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "userid",
                table: "Characters");
        }
    }
}
