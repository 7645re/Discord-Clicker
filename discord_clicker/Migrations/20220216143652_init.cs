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
                name: "Perks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Cost = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    PassiveCoefficient = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    ClickCoefficient = table.Column<long>(type: "bigint", nullable: false),
                    Tier = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Perks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nickname = table.Column<string>(type: "text", nullable: true),
                    Password = table.Column<string>(type: "text", nullable: true),
                    Money = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    Tier = table.Column<int>(type: "integer", nullable: false),
                    ClickCoefficient = table.Column<long>(type: "bigint", nullable: false),
                    PassiveCoefficient = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    LastRequestDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserPerks",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    PerkId = table.Column<int>(type: "integer", nullable: false),
                    Count = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPerks", x => new { x.PerkId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserPerks_Perks_PerkId",
                        column: x => x.PerkId,
                        principalTable: "Perks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPerks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserPerks_UserId",
                table: "UserPerks",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserPerks");

            migrationBuilder.DropTable(
                name: "Perks");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
