using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Imaginarium.Migrations
{
    public partial class second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<List<string>>(
                name: "Users",
                table: "Games",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Users",
                table: "Games");
        }
    }
}
