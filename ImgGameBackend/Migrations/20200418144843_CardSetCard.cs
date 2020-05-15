using Microsoft.EntityFrameworkCore.Migrations;

namespace Imaginarium.Migrations
{
    public partial class CardSetCard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CardSetId",
                table: "Cards",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Cards_CardSetId",
                table: "Cards",
                column: "CardSetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_CardSets_CardSetId",
                table: "Cards",
                column: "CardSetId",
                principalTable: "CardSets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_CardSets_CardSetId",
                table: "Cards");

            migrationBuilder.DropIndex(
                name: "IX_Cards_CardSetId",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "CardSetId",
                table: "Cards");
        }
    }
}
