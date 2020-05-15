using Microsoft.EntityFrameworkCore.Migrations;

namespace Imaginarium.Migrations
{
    public partial class nullable_int : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Round",
                table: "Games",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Round",
                table: "Games",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
