using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class AddNewRequestsTablesWithPaymentRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Services_ServiceId",
                table: "Payments");

            migrationBuilder.RenameColumn(
                name: "ServiceId",
                table: "Payments",
                newName: "TranscriptRequestId");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_ServiceId",
                table: "Payments",
                newName: "IX_Payments_TranscriptRequestId");

            migrationBuilder.AddColumn<string>(
                name: "CertificateRequestId",
                table: "Payments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnrollmentRequestId",
                table: "Payments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CertificateRequests",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullNameAR = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullNameEN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Language = table.Column<int>(type: "int", nullable: false),
                    ServiceId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RequestStatusId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: true),
                    StudentNumber = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CertificateRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CertificateRequests_RequestStatuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "RequestStatuses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CertificateRequests_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CertificateRequests_Students_StudentNumber",
                        column: x => x.StudentNumber,
                        principalTable: "Students",
                        principalColumn: "StudentNumber");
                });

            migrationBuilder.CreateTable(
                name: "EnrollmentRequests",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ServiceId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RequestStatusId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: true),
                    StudentNumber = table.Column<string>(type: "nvarchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnrollmentRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnrollmentRequests_RequestStatuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "RequestStatuses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EnrollmentRequests_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EnrollmentRequests_Students_StudentNumber",
                        column: x => x.StudentNumber,
                        principalTable: "Students",
                        principalColumn: "StudentNumber");
                });

            migrationBuilder.CreateTable(
                name: "TranscriptRequests",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullNameAR = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FullNameEN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Language = table.Column<int>(type: "int", nullable: false),
                    ServiceId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RequestStatusId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: true),
                    StudentNumber = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TranscriptRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TranscriptRequests_RequestStatuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "RequestStatuses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TranscriptRequests_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TranscriptRequests_Students_StudentNumber",
                        column: x => x.StudentNumber,
                        principalTable: "Students",
                        principalColumn: "StudentNumber");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CertificateRequestId",
                table: "Payments",
                column: "CertificateRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_EnrollmentRequestId",
                table: "Payments",
                column: "EnrollmentRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_CertificateRequests_ServiceId",
                table: "CertificateRequests",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_CertificateRequests_StatusId",
                table: "CertificateRequests",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_CertificateRequests_StudentNumber",
                table: "CertificateRequests",
                column: "StudentNumber");

            migrationBuilder.CreateIndex(
                name: "IX_EnrollmentRequests_ServiceId",
                table: "EnrollmentRequests",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_EnrollmentRequests_StatusId",
                table: "EnrollmentRequests",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_EnrollmentRequests_StudentNumber",
                table: "EnrollmentRequests",
                column: "StudentNumber");

            migrationBuilder.CreateIndex(
                name: "IX_TranscriptRequests_ServiceId",
                table: "TranscriptRequests",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_TranscriptRequests_StatusId",
                table: "TranscriptRequests",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_TranscriptRequests_StudentNumber",
                table: "TranscriptRequests",
                column: "StudentNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_CertificateRequests_CertificateRequestId",
                table: "Payments",
                column: "CertificateRequestId",
                principalTable: "CertificateRequests",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_EnrollmentRequests_EnrollmentRequestId",
                table: "Payments",
                column: "EnrollmentRequestId",
                principalTable: "EnrollmentRequests",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_TranscriptRequests_TranscriptRequestId",
                table: "Payments",
                column: "TranscriptRequestId",
                principalTable: "TranscriptRequests",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_CertificateRequests_CertificateRequestId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_EnrollmentRequests_EnrollmentRequestId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_TranscriptRequests_TranscriptRequestId",
                table: "Payments");

            migrationBuilder.DropTable(
                name: "CertificateRequests");

            migrationBuilder.DropTable(
                name: "EnrollmentRequests");

            migrationBuilder.DropTable(
                name: "TranscriptRequests");

            migrationBuilder.DropIndex(
                name: "IX_Payments_CertificateRequestId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_EnrollmentRequestId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "CertificateRequestId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "EnrollmentRequestId",
                table: "Payments");

            migrationBuilder.RenameColumn(
                name: "TranscriptRequestId",
                table: "Payments",
                newName: "ServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_TranscriptRequestId",
                table: "Payments",
                newName: "IX_Payments_ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Services_ServiceId",
                table: "Payments",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id");
        }
    }
}
