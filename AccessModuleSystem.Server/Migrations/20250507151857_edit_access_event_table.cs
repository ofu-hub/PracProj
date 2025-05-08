using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccessModuleSystem.Server.Migrations
{
    /// <inheritdoc />
    public partial class edit_access_event_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EventType",
                table: "AccessEvents",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AccessEvents",
                keyColumn: "Id",
                keyValue: new Guid("31e67b52-4b16-4bdc-ac3e-3c33b8040a29"),
                column: "EventType",
                value: 0);

            migrationBuilder.UpdateData(
                table: "AccessEvents",
                keyColumn: "Id",
                keyValue: new Guid("b94ff912-f540-41ad-bca5-a97b6e57f21c"),
                column: "EventType",
                value: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventType",
                table: "AccessEvents");
        }
    }
}
