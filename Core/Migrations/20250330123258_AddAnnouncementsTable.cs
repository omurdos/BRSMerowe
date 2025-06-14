using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class AddAnnouncementsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Announcement",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDisplayed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    FacultyNumber = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    DepartmentNumber = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    BatchId = table.Column<decimal>(type: "numeric(18,0)", nullable: true),
                    ProgramId = table.Column<decimal>(type: "numeric(18,0)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Announcement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Announcements_Batches",
                        column: x => x.BatchId,
                        principalTable: "batches",
                        principalColumn: "BatchID");
                    table.ForeignKey(
                        name: "FK_Announcements_Departments",
                        columns: x => new { x.FacultyNumber, x.DepartmentNumber },
                        principalTable: "Departments",
                        principalColumns: new[] { "FacultyNumber", "DepartmentNumber" });
                    table.ForeignKey(
                        name: "FK_Announcements_Faculties",
                        column: x => x.FacultyNumber,
                        principalTable: "Faculties",
                        principalColumn: "FacultyNumber");
                    table.ForeignKey(
                        name: "FK_Announcements_Program",
                        column: x => x.ProgramId,
                        principalTable: "Programs",
                        principalColumn: "ProgramID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Announcement_BatchId",
                table: "Announcement",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_Announcement_FacultyNumber_DepartmentNumber",
                table: "Announcement",
                columns: new[] { "FacultyNumber", "DepartmentNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_Announcement_ProgramId",
                table: "Announcement",
                column: "ProgramId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Announcement");
        }
    }
}
