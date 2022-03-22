using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MvManagement.Migrations
{
    public partial class VehicleDocuments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "doc");

            migrationBuilder.EnsureSchema(
                name: "veh");

            migrationBuilder.CreateTable(
                name: "tblCatInsuranceCompany",
                schema: "cat",
                columns: table => new
                {
                    IdInsuranceCompany = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsuranceCompany = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCatInsuranceCompany", x => x.IdInsuranceCompany);
                });

            migrationBuilder.CreateTable(
                name: "tblCatPeriodicalDocumentType",
                schema: "cat",
                columns: table => new
                {
                    IdPeriodicalDocumentType = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PeriodicalDocumentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCatPeriodicalDocumentType", x => x.IdPeriodicalDocumentType);
                });

            migrationBuilder.CreateTable(
                name: "tblCatStorageDocumentType",
                schema: "cat",
                columns: table => new
                {
                    IdStorageDocumentType = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StorageDocumentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCatStorageDocumentType", x => x.IdStorageDocumentType);
                });

            migrationBuilder.CreateTable(
                name: "tblVehicle",
                schema: "veh",
                columns: table => new
                {
                    IdVehicle = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleTitle = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ProductionYear = table.Column<int>(type: "int", nullable: false),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: true),
                    ChassisNumber = table.Column<string>(type: "nvarchar(17)", maxLength: 17, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblVehicle", x => x.IdVehicle);
                });

            migrationBuilder.CreateTable(
                name: "tblVehicleRole",
                schema: "veh",
                columns: table => new
                {
                    IdVehicleRole = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblVehicleRole", x => x.IdVehicleRole);
                });

            migrationBuilder.CreateTable(
                name: "tblVehicleRoleUser",
                schema: "veh",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    IdVehicleRole = table.Column<int>(type: "int", nullable: false),
                    IdVehicle = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblVehicleRoleUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblVehicleRoleUser_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "tblInsuranceDocuments",
                schema: "doc",
                columns: table => new
                {
                    IdInsuranceDocument = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsuranceType = table.Column<int>(type: "int", nullable: false),
                    InsurancePolicyNumber = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    IdInsuranceCompany = table.Column<int>(type: "int", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdVehicle = table.Column<long>(type: "bigint", nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidTo = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblInsuranceDocuments", x => x.IdInsuranceDocument);
                    table.ForeignKey(
                        name: "FK_tblInsuranceDocuments_tblCatInsuranceCompany_IdInsuranceCompany",
                        column: x => x.IdInsuranceCompany,
                        principalSchema: "cat",
                        principalTable: "tblCatInsuranceCompany",
                        principalColumn: "IdInsuranceCompany");
                    table.ForeignKey(
                        name: "FK_tblInsuranceDocuments_tblVehicle_IdVehicle",
                        column: x => x.IdVehicle,
                        principalSchema: "veh",
                        principalTable: "tblVehicle",
                        principalColumn: "IdVehicle",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblPeriodicalDocument",
                schema: "doc",
                columns: table => new
                {
                    IdPeriodicalDocument = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdPeriodicalDocumentType = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdVehicle = table.Column<long>(type: "bigint", nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidTo = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblPeriodicalDocument", x => x.IdPeriodicalDocument);
                    table.ForeignKey(
                        name: "FK_tblPeriodicalDocument_tblCatPeriodicalDocumentType_IdPeriodicalDocumentType",
                        column: x => x.IdPeriodicalDocumentType,
                        principalSchema: "cat",
                        principalTable: "tblCatPeriodicalDocumentType",
                        principalColumn: "IdPeriodicalDocumentType",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblPeriodicalDocument_tblVehicle_IdVehicle",
                        column: x => x.IdVehicle,
                        principalSchema: "veh",
                        principalTable: "tblVehicle",
                        principalColumn: "IdVehicle",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblStorageDocument",
                schema: "doc",
                columns: table => new
                {
                    IdStorageDocument = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdStorageDocumentType = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdVehicle = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblStorageDocument", x => x.IdStorageDocument);
                    table.ForeignKey(
                        name: "FK_tblStorageDocument_tblCatStorageDocumentType_IdStorageDocumentType",
                        column: x => x.IdStorageDocumentType,
                        principalSchema: "cat",
                        principalTable: "tblCatStorageDocumentType",
                        principalColumn: "IdStorageDocumentType",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblStorageDocument_tblVehicle_IdVehicle",
                        column: x => x.IdVehicle,
                        principalSchema: "veh",
                        principalTable: "tblVehicle",
                        principalColumn: "IdVehicle",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblVehiclePermission",
                schema: "veh",
                columns: table => new
                {
                    IdVehiclePermission = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PermissionDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdVehicle = table.Column<long>(type: "bigint", nullable: true),
                    IdVehicleRole = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblVehiclePermission", x => x.IdVehiclePermission);
                    table.ForeignKey(
                        name: "FK_tblVehiclePermission_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_tblVehiclePermission_tblVehicle_IdVehicle",
                        column: x => x.IdVehicle,
                        principalSchema: "veh",
                        principalTable: "tblVehicle",
                        principalColumn: "IdVehicle");
                    table.ForeignKey(
                        name: "FK_tblVehiclePermission_tblVehicleRole_IdVehicleRole",
                        column: x => x.IdVehicleRole,
                        principalSchema: "veh",
                        principalTable: "tblVehicleRole",
                        principalColumn: "IdVehicleRole");
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblInsuranceDocuments_IdInsuranceCompany",
                schema: "doc",
                table: "tblInsuranceDocuments",
                column: "IdInsuranceCompany");

            migrationBuilder.CreateIndex(
                name: "IX_tblInsuranceDocuments_IdVehicle",
                schema: "doc",
                table: "tblInsuranceDocuments",
                column: "IdVehicle");

            migrationBuilder.CreateIndex(
                name: "IX_tblPeriodicalDocument_IdPeriodicalDocumentType",
                schema: "doc",
                table: "tblPeriodicalDocument",
                column: "IdPeriodicalDocumentType");

            migrationBuilder.CreateIndex(
                name: "IX_tblPeriodicalDocument_IdVehicle",
                schema: "doc",
                table: "tblPeriodicalDocument",
                column: "IdVehicle");

            migrationBuilder.CreateIndex(
                name: "IX_tblStorageDocument_IdStorageDocumentType",
                schema: "doc",
                table: "tblStorageDocument",
                column: "IdStorageDocumentType");

            migrationBuilder.CreateIndex(
                name: "IX_tblStorageDocument_IdVehicle",
                schema: "doc",
                table: "tblStorageDocument",
                column: "IdVehicle");

            migrationBuilder.CreateIndex(
                name: "IX_tblVehiclePermission_IdVehicle",
                schema: "veh",
                table: "tblVehiclePermission",
                column: "IdVehicle");

            migrationBuilder.CreateIndex(
                name: "IX_tblVehiclePermission_IdVehicleRole",
                schema: "veh",
                table: "tblVehiclePermission",
                column: "IdVehicleRole");

            migrationBuilder.CreateIndex(
                name: "IX_tblVehiclePermission_UserId",
                schema: "veh",
                table: "tblVehiclePermission",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_tblVehicleRoleUser_UserId_IdVehicle",
                schema: "veh",
                table: "tblVehicleRoleUser",
                columns: new[] { "UserId", "IdVehicle" },
                unique: true,
                filter: "[UserId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblInsuranceDocuments",
                schema: "doc");

            migrationBuilder.DropTable(
                name: "tblPeriodicalDocument",
                schema: "doc");

            migrationBuilder.DropTable(
                name: "tblStorageDocument",
                schema: "doc");

            migrationBuilder.DropTable(
                name: "tblVehiclePermission",
                schema: "veh");

            migrationBuilder.DropTable(
                name: "tblVehicleRoleUser",
                schema: "veh");

            migrationBuilder.DropTable(
                name: "tblCatInsuranceCompany",
                schema: "cat");

            migrationBuilder.DropTable(
                name: "tblCatPeriodicalDocumentType",
                schema: "cat");

            migrationBuilder.DropTable(
                name: "tblCatStorageDocumentType",
                schema: "cat");

            migrationBuilder.DropTable(
                name: "tblVehicle",
                schema: "veh");

            migrationBuilder.DropTable(
                name: "tblVehicleRole",
                schema: "veh");
        }
    }
}
