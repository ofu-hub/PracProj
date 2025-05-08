using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccessModuleSystem.Server.Migrations
{
    /// <inheritdoc />
    public partial class add_screenshot_access_event : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Screenshot",
                table: "AccessEvents",
                type: "bytea",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AccessEvents",
                keyColumn: "Id",
                keyValue: new Guid("31e67b52-4b16-4bdc-ac3e-3c33b8013a13"),
                column: "Screenshot",
                value: null);

            migrationBuilder.UpdateData(
                table: "AccessEvents",
                keyColumn: "Id",
                keyValue: new Guid("31e67b52-4b16-4bdc-ac3e-3c33b8040a29"),
                column: "Screenshot",
                value: null);

            migrationBuilder.UpdateData(
                table: "AccessEvents",
                keyColumn: "Id",
                keyValue: new Guid("b94ff912-f540-41ad-bca5-a97b6e57f21c"),
                column: "Screenshot",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Screenshot",
                table: "AccessEvents");
        }
    }
}
