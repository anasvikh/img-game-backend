using Microsoft.EntityFrameworkCore.Migrations;

namespace Imaginarium.Migrations
{
    public partial class RenameChipId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChipColor",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "ChipId",
                table: "Users",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChipId",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "ChipColor",
                table: "Users",
                nullable: false,
                defaultValue: 0);
        }
    }
}
