using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccessModuleSystem.Server.Migrations
{
    /// <inheritdoc />
    public partial class edit_access_event_vehicleid_not_req : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "VehicleId",
                table: "AccessEvents",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.UpdateData(
                table: "AccessEvents",
                keyColumn: "Id",
                keyValue: new Guid("31e67b52-4b16-4bdc-ac3e-3c33b8013a13"),
                column: "VehicleId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d1b0f2cc-79b3-4b69-928c-8b263f2ab9c4"),
                columns: new[] { "Name", "Surname" },
                values: new object[] { "Марк", "Власов" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "VehicleId",
                table: "AccessEvents",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AccessEvents",
                keyColumn: "Id",
                keyValue: new Guid("31e67b52-4b16-4bdc-ac3e-3c33b8013a13"),
                column: "VehicleId",
                value: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d1b0f2cc-79b3-4b69-928c-8b263f2ab9c4"),
                columns: new[] { "Name", "Surname" },
                values: new object[] { "Павел", "Маркелов" });
        }
    }
}
