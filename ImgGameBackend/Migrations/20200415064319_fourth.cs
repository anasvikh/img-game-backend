using Microsoft.EntityFrameworkCore.Migrations;

namespace Imaginarium.Migrations
{
    public partial class fourth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GameId",
                table: "PlayCard",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PlayCard_GameId",
                table: "PlayCard",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayCard_Games_GameId",
                table: "PlayCard",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayCard_Games_GameId",
                table: "PlayCard");

            migrationBuilder.DropIndex(
                name: "IX_PlayCard_GameId",
                table: "PlayCard");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "PlayCard");
        }
    }
}
