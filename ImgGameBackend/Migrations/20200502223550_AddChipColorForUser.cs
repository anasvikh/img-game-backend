using Microsoft.EntityFrameworkCore.Migrations;

namespace Imaginarium.Migrations
{
    public partial class AddChipColorForUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PlayCards_GameId",
                table: "PlayCards");

            migrationBuilder.AddColumn<int>(
                name: "ChipColor",
                table: "Users",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PlayCards_GameId_CardId",
                table: "PlayCards",
                columns: new[] { "GameId", "CardId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PlayCards_GameId_CardId",
                table: "PlayCards");

            migrationBuilder.DropColumn(
                name: "ChipColor",
                table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_PlayCards_GameId",
                table: "PlayCards",
                column: "GameId");
        }
    }
}
