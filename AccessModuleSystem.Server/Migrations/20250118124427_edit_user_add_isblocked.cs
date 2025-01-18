using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccessModuleSystem.Server.Migrations
{
    /// <inheritdoc />
    public partial class edit_user_add_isblocked : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBlocked",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d1b0f2cc-79b3-4b69-928c-8b263f2ab9c4"),
                column: "IsBlocked",
                value: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d1b0f4cc-79b3-4b69-988c-8b263f2ab9c4"),
                column: "IsBlocked",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBlocked",
                table: "Users");
        }
    }
}
