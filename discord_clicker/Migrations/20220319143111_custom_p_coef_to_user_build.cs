using Microsoft.EntityFrameworkCore.Migrations;

namespace discord_clicker.Migrations
{
    public partial class custom_p_coef_to_user_build : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PassiveCoefficient",
                table: "UserBuilds",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PassiveCoefficient",
                table: "UserBuilds");
        }
    }
}
