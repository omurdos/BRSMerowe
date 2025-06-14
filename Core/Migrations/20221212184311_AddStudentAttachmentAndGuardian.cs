using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class AddStudentAttachmentAndGuardian : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdmissionTypeId",
                table: "Students",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Guardians",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentNumber = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guardians", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Guardians_Students_StudentNumber",
                        column: x => x.StudentNumber,
                        principalTable: "Students",
                        principalColumn: "StudentNumber");
                });

            migrationBuilder.CreateTable(
                name: "StudentAttachments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PersonalPhoto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MedicalReport = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdentityProof = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StudentNumber = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentAttachments_Students_StudentNumber",
                        column: x => x.StudentNumber,
                        principalTable: "Students",
                        principalColumn: "StudentNumber");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Students_AdmissionTypeId",
                table: "Students",
                column: "AdmissionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Guardians_StudentNumber",
                table: "Guardians",
                column: "StudentNumber",
                unique: true,
                filter: "[StudentNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAttachments_StudentNumber",
                table: "StudentAttachments",
                column: "StudentNumber",
                unique: true,
                filter: "[StudentNumber] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_AdmissionTypes_AdmissionTypeId",
                table: "Students",
                column: "AdmissionTypeId",
                principalTable: "AdmissionTypes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_AdmissionTypes_AdmissionTypeId",
                table: "Students");

            migrationBuilder.DropTable(
                name: "Guardians");

            migrationBuilder.DropTable(
                name: "StudentAttachments");

            migrationBuilder.DropIndex(
                name: "IX_Students_AdmissionTypeId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "AdmissionTypeId",
                table: "Students");
        }
    }
}
