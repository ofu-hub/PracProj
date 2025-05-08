using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccessModuleSystem.Server.Migrations
{
    /// <inheritdoc />
    public partial class fix_vehicle_foreign_key : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Vehicles",
                columns: new[] { "Id", "CreatedAt", "DeactivationAt", "LicensePlate", "OwnerName", "Status", "UserId" },
                values: new object[] { new Guid("8a7cb1e2-3c3b-4f8e-91e3-45b0b8b70c00"), new DateTime(2023, 5, 16, 0, 37, 19, 0, DateTimeKind.Utc), null, "О333ОО 30 RUS", "Неизвестно", 0, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: new Guid("8a7cb1e2-3c3b-4f8e-91e3-45b0b8b70c00"));
        }
    }
}
