using Microsoft.EntityFrameworkCore.Migrations;

namespace discord_clicker.Migrations
{
    public partial class add_get_money_to_upgrade_model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Get",
                table: "Upgrades",
                newName: "GetMoney");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GetMoney",
                table: "Upgrades",
                newName: "Get");
        }
    }
}
