using Microsoft.EntityFrameworkCore.Migrations;

namespace Imaginarium.Migrations
{
    public partial class five : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayCard_Card_CardId",
                table: "PlayCard");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayCard_Games_GameId",
                table: "PlayCard");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayCard",
                table: "PlayCard");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Card",
                table: "Card");

            migrationBuilder.RenameTable(
                name: "PlayCard",
                newName: "PlayCards");

            migrationBuilder.RenameTable(
                name: "Card",
                newName: "Cards");

            migrationBuilder.RenameIndex(
                name: "IX_PlayCard_GameId",
                table: "PlayCards",
                newName: "IX_PlayCards_GameId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayCard_CardId",
                table: "PlayCards",
                newName: "IX_PlayCards_CardId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayCards",
                table: "PlayCards",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cards",
                table: "Cards",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayCards_Cards_CardId",
                table: "PlayCards",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayCards_Games_GameId",
                table: "PlayCards",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayCards_Cards_CardId",
                table: "PlayCards");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayCards_Games_GameId",
                table: "PlayCards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayCards",
                table: "PlayCards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Cards",
                table: "Cards");

            migrationBuilder.RenameTable(
                name: "PlayCards",
                newName: "PlayCard");

            migrationBuilder.RenameTable(
                name: "Cards",
                newName: "Card");

            migrationBuilder.RenameIndex(
                name: "IX_PlayCards_GameId",
                table: "PlayCard",
                newName: "IX_PlayCard_GameId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayCards_CardId",
                table: "PlayCard",
                newName: "IX_PlayCard_CardId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayCard",
                table: "PlayCard",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Card",
                table: "Card",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayCard_Card_CardId",
                table: "PlayCard",
                column: "CardId",
                principalTable: "Card",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayCard_Games_GameId",
                table: "PlayCard",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
