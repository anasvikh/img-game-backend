using Microsoft.EntityFrameworkCore.Migrations;

namespace Imaginarium.Migrations
{
    public partial class AddClassicCardSet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO \"CardSets\" (\"Id\", \"NameEng\", \"NameRus\") VALUES ((SELECT COALESCE(MAX(\"Id\"), 0) FROM \"CardSets\")+1, 'classic', 'Классический')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM CardSets WHERE NameEng = classic");
        }
    }
}
