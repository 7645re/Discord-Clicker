using Microsoft.EntityFrameworkCore.Migrations;

namespace discord_clicker.Migrations
{
    public partial class g : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ClickCoefficient",
                table: "Perks",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClickCoefficient",
                table: "Perks");
        }
    }
}
