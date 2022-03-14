using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MvManagement.Migrations
{
    public partial class VehicleOwner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                schema: "veh",
                table: "tblVehicle",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                schema: "veh",
                table: "tblVehicle",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tblVehicle_UserId",
                schema: "veh",
                table: "tblVehicle",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblVehicle_AbpUsers_UserId",
                schema: "veh",
                table: "tblVehicle",
                column: "UserId",
                principalTable: "AbpUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblVehicle_AbpUsers_UserId",
                schema: "veh",
                table: "tblVehicle");

            migrationBuilder.DropIndex(
                name: "IX_tblVehicle_UserId",
                schema: "veh",
                table: "tblVehicle");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "veh",
                table: "tblVehicle");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "veh",
                table: "tblVehicle");
        }
    }
}
