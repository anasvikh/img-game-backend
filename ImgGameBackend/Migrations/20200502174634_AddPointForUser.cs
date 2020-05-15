using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Imaginarium.Migrations
{
    public partial class AddPointForUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Users",
                table: "Games");

            migrationBuilder.AddColumn<string>(
                name: "ConnectionId",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GameId",
                table: "Users",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Points",
                table: "Users",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Users_GameId",
                table: "Users",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Games_GameId",
                table: "Users",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Games_GameId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_GameId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ConnectionId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Points",
                table: "Users");

            migrationBuilder.AddColumn<List<string>>(
                name: "Users",
                table: "Games",
                nullable: true);
        }
    }
}
