using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccessModuleSystem.Server.Migrations
{
    /// <inheritdoc />
    public partial class add_new_item : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AccessEvents",
                columns: new[] { "Id", "AccessType", "CameraId", "EventType", "Status", "Timestamp", "VehicleId" },
                values: new object[] { new Guid("31e67b52-4b16-4bdc-ac3e-3c33b8013a13"), 0, new Guid("dbc4b6ba-d49f-4785-8ba0-14516620ae66"), 1, 0, new DateTime(2023, 5, 16, 0, 37, 19, 0, DateTimeKind.Utc), new Guid("5b037b65-19f1-402a-ad5b-9779ef098b19") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AccessEvents",
                keyColumn: "Id",
                keyValue: new Guid("31e67b52-4b16-4bdc-ac3e-3c33b8013a13"));
        }
    }
}
