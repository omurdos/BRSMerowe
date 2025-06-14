using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class AddAnnouncementsTbl : Migration
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
                    FacultyNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FacultyNumber1 = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    DepartmentNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepartmentFacultyNumber = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    DepartmentNumber1 = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    BatchId = table.Column<decimal>(type: "numeric(18,0)", nullable: false),
                    ProgramId = table.Column<decimal>(type: "numeric(18,0)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Announcement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Announcement_batches_BatchId",
                        column: x => x.BatchId,
                        principalTable: "batches",
                        principalColumn: "BatchID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Announcement_Departments_DepartmentFacultyNumber_DepartmentNumber1",
                        columns: x => new { x.DepartmentFacultyNumber, x.DepartmentNumber1 },
                        principalTable: "Departments",
                        principalColumns: new[] { "FacultyNumber", "DepartmentNumber" });
                    table.ForeignKey(
                        name: "FK_Announcement_Faculties_FacultyNumber1",
                        column: x => x.FacultyNumber1,
                        principalTable: "Faculties",
                        principalColumn: "FacultyNumber");
                    table.ForeignKey(
                        name: "FK_Announcement_Programs_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "Programs",
                        principalColumn: "ProgramID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Announcement_BatchId",
                table: "Announcement",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_Announcement_DepartmentFacultyNumber_DepartmentNumber1",
                table: "Announcement",
                columns: new[] { "DepartmentFacultyNumber", "DepartmentNumber1" });

            migrationBuilder.CreateIndex(
                name: "IX_Announcement_FacultyNumber1",
                table: "Announcement",
                column: "FacultyNumber1");

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
