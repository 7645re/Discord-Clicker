using Microsoft.EntityFrameworkCore.Migrations;

namespace discord_clicker.Migrations
{
    public partial class add_count_to_achievement_for_polyform_interface : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAchievements_Achievements_AchievementId",
                table: "UserAchievements");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBuilds_Builds_BuildId",
                table: "UserBuilds");

            migrationBuilder.DropForeignKey(
                name: "FK_UserUpgrades_Upgrades_UpgradeId",
                table: "UserUpgrades");

            migrationBuilder.RenameColumn(
                name: "UpgradeId",
                table: "UserUpgrades",
                newName: "ItemId");

            migrationBuilder.RenameColumn(
                name: "BuildId",
                table: "UserBuilds",
                newName: "ItemId");

            migrationBuilder.RenameColumn(
                name: "AchievementId",
                table: "UserAchievements",
                newName: "ItemId");

            migrationBuilder.AddColumn<long>(
                name: "Count",
                table: "UserAchievements",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAchievements_Achievements_ItemId",
                table: "UserAchievements",
                column: "ItemId",
                principalTable: "Achievements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserBuilds_Builds_ItemId",
                table: "UserBuilds",
                column: "ItemId",
                principalTable: "Builds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserUpgrades_Upgrades_ItemId",
                table: "UserUpgrades",
                column: "ItemId",
                principalTable: "Upgrades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAchievements_Achievements_ItemId",
                table: "UserAchievements");

            migrationBuilder.DropForeignKey(
                name: "FK_UserBuilds_Builds_ItemId",
                table: "UserBuilds");

            migrationBuilder.DropForeignKey(
                name: "FK_UserUpgrades_Upgrades_ItemId",
                table: "UserUpgrades");

            migrationBuilder.DropColumn(
                name: "Count",
                table: "UserAchievements");

            migrationBuilder.RenameColumn(
                name: "ItemId",
                table: "UserUpgrades",
                newName: "UpgradeId");

            migrationBuilder.RenameColumn(
                name: "ItemId",
                table: "UserBuilds",
                newName: "BuildId");

            migrationBuilder.RenameColumn(
                name: "ItemId",
                table: "UserAchievements",
                newName: "AchievementId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAchievements_Achievements_AchievementId",
                table: "UserAchievements",
                column: "AchievementId",
                principalTable: "Achievements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserBuilds_Builds_BuildId",
                table: "UserBuilds",
                column: "BuildId",
                principalTable: "Builds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserUpgrades_Upgrades_UpgradeId",
                table: "UserUpgrades",
                column: "UpgradeId",
                principalTable: "Upgrades",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
