using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace discord_clicker.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Achievements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    AchievementObject = table.Column<string>(type: "text", nullable: true),
                    AchievementObjectCount = table.Column<decimal>(type: "numeric", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Achievements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Builds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Cost = table.Column<decimal>(type: "numeric", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    PassiveCoefficient = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Builds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Upgrades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Cost = table.Column<decimal>(type: "numeric", nullable: false),
                    BuildId = table.Column<long>(type: "bigint", nullable: false),
                    Action = table.Column<string>(type: "text", nullable: true),
                    ConditionGet = table.Column<long>(type: "bigint", nullable: false),
                    ForEachBuild = table.Column<bool>(type: "boolean", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Upgrades", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Money = table.Column<decimal>(type: "numeric", nullable: false),
                    AllMoney = table.Column<decimal>(type: "numeric", nullable: false),
                    Nickname = table.Column<string>(type: "text", nullable: true),
                    Password = table.Column<string>(type: "text", nullable: true),
                    Click = table.Column<decimal>(type: "numeric", nullable: false),
                    PlayStartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    LastRequestDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ClickCoefficient = table.Column<decimal>(type: "numeric", nullable: false),
                    PassiveCoefficient = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserAchievements",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    AchievementId = table.Column<int>(type: "integer", nullable: false),
                    DateOfachievement = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAchievements", x => new { x.AchievementId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserAchievements_Achievements_AchievementId",
                        column: x => x.AchievementId,
                        principalTable: "Achievements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAchievements_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserBuilds",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    BuildId = table.Column<int>(type: "integer", nullable: false),
                    Count = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserBuilds", x => new { x.BuildId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserBuilds_Builds_BuildId",
                        column: x => x.BuildId,
                        principalTable: "Builds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserBuilds_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserUpgrades",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    UpgradeId = table.Column<int>(type: "integer", nullable: false),
                    Count = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserUpgrades", x => new { x.UpgradeId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserUpgrades_Upgrades_UpgradeId",
                        column: x => x.UpgradeId,
                        principalTable: "Upgrades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserUpgrades_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserAchievements_UserId",
                table: "UserAchievements",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserBuilds_UserId",
                table: "UserBuilds",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserUpgrades_UserId",
                table: "UserUpgrades",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAchievements");

            migrationBuilder.DropTable(
                name: "UserBuilds");

            migrationBuilder.DropTable(
                name: "UserUpgrades");

            migrationBuilder.DropTable(
                name: "Achievements");

            migrationBuilder.DropTable(
                name: "Builds");

            migrationBuilder.DropTable(
                name: "Upgrades");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
