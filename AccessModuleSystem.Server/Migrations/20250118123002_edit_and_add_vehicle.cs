using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AccessModuleSystem.Server.Migrations
{
    /// <inheritdoc />
    public partial class edit_and_add_vehicle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "VehicleId",
                table: "Users",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Cameras",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Location = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cameras", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LicensePlate = table.Column<string>(type: "text", nullable: false),
                    OwnerName = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeactivationAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vehicles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AccessEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uuid", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AccessType = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CameraId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccessEvents_Cameras_CameraId",
                        column: x => x.CameraId,
                        principalTable: "Cameras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccessEvents_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d1b0f2cc-79b3-4b69-928c-8b263f2ab9c4"),
                column: "VehicleId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("d1b0f4cc-79b3-4b69-988c-8b263f2ab9c4"),
                column: "VehicleId",
                value: null);

            migrationBuilder.InsertData(
                table: "Vehicles",
                columns: new[] { "Id", "CreatedAt", "DeactivationAt", "LicensePlate", "OwnerName", "Status", "UserId" },
                values: new object[,]
                {
                    { new Guid("d1b0f2cc-12b3-4b69-928c-8b263f3ab9c4"), new DateTime(2023, 5, 16, 0, 37, 19, 0, DateTimeKind.Utc), null, "A123BC 30 RUS", "Иванов Иван Иванович", 0, null },
                    { new Guid("d1b0f2cc-14b3-4b69-928c-8b263f3ab9c4"), new DateTime(2023, 5, 16, 0, 37, 19, 0, DateTimeKind.Utc), null, "A726BC 30 RUS", "Петров Петр", 0, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_VehicleId",
                table: "Users",
                column: "VehicleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccessEvents_CameraId",
                table: "AccessEvents",
                column: "CameraId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessEvents_VehicleId",
                table: "AccessEvents",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_UserId",
                table: "Vehicles",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Vehicles_VehicleId",
                table: "Users",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Vehicles_VehicleId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "AccessEvents");

            migrationBuilder.DropTable(
                name: "Cameras");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Users_VehicleId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "VehicleId",
                table: "Users");
        }
    }
}
