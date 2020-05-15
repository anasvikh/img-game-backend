using Microsoft.EntityFrameworkCore.Migrations;

namespace Imaginarium.Migrations
{
    public partial class NullableRoud : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Round",
                table: "PlayCards",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "Round",
                table: "Games",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Round",
                table: "PlayCards",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Round",
                table: "Games",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
