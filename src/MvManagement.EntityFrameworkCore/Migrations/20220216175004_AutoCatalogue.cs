using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MvManagement.Migrations
{
    public partial class AutoCatalogue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "cat");

            migrationBuilder.CreateTable(
                name: "tblCatAutoMake",
                schema: "cat",
                columns: table => new
                {
                    IdMake = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdExtern = table.Column<int>(type: "int", nullable: false),
                    Provider = table.Column<int>(type: "int", nullable: false),
                    Make = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCatAutoMake", x => x.IdMake);
                });

            migrationBuilder.CreateTable(
                name: "tblCatAutoMakeCategory",
                schema: "cat",
                columns: table => new
                {
                    IdMakeCategory = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdMake = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCatAutoMakeCategory", x => x.IdMakeCategory);
                    table.ForeignKey(
                        name: "FK_tblCatAutoMakeCategory_tblCatAutoMake_IdMake",
                        column: x => x.IdMake,
                        principalSchema: "cat",
                        principalTable: "tblCatAutoMake",
                        principalColumn: "IdMake",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblCatAutoModel",
                schema: "cat",
                columns: table => new
                {
                    IdModel = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Model = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    IdMake = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCatAutoModel", x => x.IdModel);
                    table.ForeignKey(
                        name: "FK_tblCatAutoModel_tblCatAutoMake_IdMake",
                        column: x => x.IdMake,
                        principalSchema: "cat",
                        principalTable: "tblCatAutoMake",
                        principalColumn: "IdMake",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblCatAutoMakeCategory_IdMake",
                schema: "cat",
                table: "tblCatAutoMakeCategory",
                column: "IdMake");

            migrationBuilder.CreateIndex(
                name: "IX_tblCatAutoModel_IdMake",
                schema: "cat",
                table: "tblCatAutoModel",
                column: "IdMake");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblCatAutoMakeCategory",
                schema: "cat");

            migrationBuilder.DropTable(
                name: "tblCatAutoModel",
                schema: "cat");

            migrationBuilder.DropTable(
                name: "tblCatAutoMake",
                schema: "cat");
        }
    }
}
