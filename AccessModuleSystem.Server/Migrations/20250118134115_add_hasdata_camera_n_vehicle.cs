using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AccessModuleSystem.Server.Migrations
{
    /// <inheritdoc />
    public partial class add_hasdata_camera_n_vehicle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: new Guid("d1b0f2cc-12b3-4b69-928c-8b263f3ab9c4"));

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: new Guid("d1b0f2cc-14b3-4b69-928c-8b263f3ab9c4"));

            migrationBuilder.InsertData(
                table: "Cameras",
                columns: new[] { "Id", "Location", "Status" },
                values: new object[,]
                {
                    { new Guid("31376f03-ffed-427b-a5cf-e7a04105d689"), "ул. Пушкина, д.1, к.1", 2 },
                    { new Guid("dbc4b6ba-d49f-4785-8ba0-14516620ae66"), "ул. Пушкина, д.2, к.1", 0 }
                });

            migrationBuilder.InsertData(
                table: "Vehicles",
                columns: new[] { "Id", "CreatedAt", "DeactivationAt", "LicensePlate", "OwnerName", "Status", "UserId" },
                values: new object[,]
                {
                    { new Guid("15e9434f-0499-4b67-9855-8b82379bf458"), new DateTime(2023, 5, 16, 0, 37, 19, 0, DateTimeKind.Utc), null, "A726BC 30 RUS", "Петров Петр", 0, null },
                    { new Guid("5b037b65-19f1-402a-ad5b-9779ef098b19"), new DateTime(2023, 5, 16, 0, 37, 19, 0, DateTimeKind.Utc), null, "A123BC 30 RUS", "Иванов Иван Иванович", 0, null }
                });

            migrationBuilder.InsertData(
                table: "AccessEvents",
                columns: new[] { "Id", "AccessType", "CameraId", "Status", "Timestamp", "VehicleId" },
                values: new object[,]
                {
                    { new Guid("31e67b52-4b16-4bdc-ac3e-3c33b8040a29"), 0, new Guid("dbc4b6ba-d49f-4785-8ba0-14516620ae66"), 0, new DateTime(2023, 5, 16, 0, 37, 19, 0, DateTimeKind.Utc), new Guid("5b037b65-19f1-402a-ad5b-9779ef098b19") },
                    { new Guid("b94ff912-f540-41ad-bca5-a97b6e57f21c"), 0, new Guid("31376f03-ffed-427b-a5cf-e7a04105d689"), 0, new DateTime(2023, 5, 16, 0, 37, 19, 0, DateTimeKind.Utc), new Guid("15e9434f-0499-4b67-9855-8b82379bf458") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AccessEvents",
                keyColumn: "Id",
                keyValue: new Guid("31e67b52-4b16-4bdc-ac3e-3c33b8040a29"));

            migrationBuilder.DeleteData(
                table: "AccessEvents",
                keyColumn: "Id",
                keyValue: new Guid("b94ff912-f540-41ad-bca5-a97b6e57f21c"));

            migrationBuilder.DeleteData(
                table: "Cameras",
                keyColumn: "Id",
                keyValue: new Guid("31376f03-ffed-427b-a5cf-e7a04105d689"));

            migrationBuilder.DeleteData(
                table: "Cameras",
                keyColumn: "Id",
                keyValue: new Guid("dbc4b6ba-d49f-4785-8ba0-14516620ae66"));

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: new Guid("15e9434f-0499-4b67-9855-8b82379bf458"));

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: new Guid("5b037b65-19f1-402a-ad5b-9779ef098b19"));

            migrationBuilder.InsertData(
                table: "Vehicles",
                columns: new[] { "Id", "CreatedAt", "DeactivationAt", "LicensePlate", "OwnerName", "Status", "UserId" },
                values: new object[,]
                {
                    { new Guid("d1b0f2cc-12b3-4b69-928c-8b263f3ab9c4"), new DateTime(2023, 5, 16, 0, 37, 19, 0, DateTimeKind.Utc), null, "A123BC 30 RUS", "Иванов Иван Иванович", 0, null },
                    { new Guid("d1b0f2cc-14b3-4b69-928c-8b263f3ab9c4"), new DateTime(2023, 5, 16, 0, 37, 19, 0, DateTimeKind.Utc), null, "A726BC 30 RUS", "Петров Петр", 0, null }
                });
        }
    }
}
