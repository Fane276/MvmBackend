using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MvManagement.Migrations
{
    public partial class FuelManagement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblFuelRefill",
                schema: "veh",
                columns: table => new
                {
                    IdFuelRefill = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FuelType = table.Column<int>(type: "int", nullable: false),
                    FuelAmount = table.Column<double>(type: "float", nullable: false),
                    FuelUnit = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    IdVehicle = table.Column<long>(type: "bigint", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblFuelRefill", x => x.IdFuelRefill);
                    table.ForeignKey(
                        name: "FK_tblFuelRefill_tblVehicle_IdVehicle",
                        column: x => x.IdVehicle,
                        principalSchema: "veh",
                        principalTable: "tblVehicle",
                        principalColumn: "IdVehicle",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblFuelRefill_IdVehicle",
                schema: "veh",
                table: "tblFuelRefill",
                column: "IdVehicle");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblFuelRefill",
                schema: "veh");
        }
    }
}
