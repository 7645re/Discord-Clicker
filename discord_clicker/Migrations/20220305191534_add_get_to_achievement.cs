﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace discord_clicker.Migrations
{
    public partial class add_get_to_achievement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Get",
                table: "Upgrades",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Get",
                table: "Upgrades");
        }
    }
}