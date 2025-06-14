using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class ReCreateCardRequestsAndPaymentsTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Payment_PaymentId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_CertificateRequests_CertificateRequestId",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_EnrollmentRequests_EnrollmentRequestId",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Students_StudentNumber",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_TranscriptRequests_TranscriptRequestId",
                table: "Payment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payment",
                table: "Payment");

            migrationBuilder.RenameTable(
                name: "Payment",
                newName: "Payments");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_TranscriptRequestId",
                table: "Payments",
                newName: "IX_Payments_TranscriptRequestId");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_StudentNumber",
                table: "Payments",
                newName: "IX_Payments_StudentNumber");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_EnrollmentRequestId",
                table: "Payments",
                newName: "IX_Payments_EnrollmentRequestId");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_CertificateRequestId",
                table: "Payments",
                newName: "IX_Payments_CertificateRequestId");

            migrationBuilder.AddColumn<string>(
                name: "CardRequestId",
                table: "Payments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payments",
                table: "Payments",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "CardRequests",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ServiceId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RequestStatusId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusId = table.Column<int>(type: "int", nullable: true),
                    StudentNumber = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardRequests_RequestStatuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "RequestStatuses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CardRequests_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CardRequests_Students_StudentNumber",
                        column: x => x.StudentNumber,
                        principalTable: "Students",
                        principalColumn: "StudentNumber");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CardRequestId",
                table: "Payments",
                column: "CardRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_CardRequests_ServiceId",
                table: "CardRequests",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_CardRequests_StatusId",
                table: "CardRequests",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_CardRequests_StudentNumber",
                table: "CardRequests",
                column: "StudentNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Payments_PaymentId",
                table: "Invoices",
                column: "PaymentId",
                principalTable: "Payments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_CardRequests_CardRequestId",
                table: "Payments",
                column: "CardRequestId",
                principalTable: "CardRequests",
                principalColumn: "Id");

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
                name: "FK_Payments_Students_StudentNumber",
                table: "Payments",
                column: "StudentNumber",
                principalTable: "Students",
                principalColumn: "StudentNumber");

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
                name: "FK_Invoices_Payments_PaymentId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_CardRequests_CardRequestId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_CertificateRequests_CertificateRequestId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_EnrollmentRequests_EnrollmentRequestId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Students_StudentNumber",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_TranscriptRequests_TranscriptRequestId",
                table: "Payments");

            migrationBuilder.DropTable(
                name: "CardRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payments",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_CardRequestId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "CardRequestId",
                table: "Payments");

            migrationBuilder.RenameTable(
                name: "Payments",
                newName: "Payment");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_TranscriptRequestId",
                table: "Payment",
                newName: "IX_Payment_TranscriptRequestId");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_StudentNumber",
                table: "Payment",
                newName: "IX_Payment_StudentNumber");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_EnrollmentRequestId",
                table: "Payment",
                newName: "IX_Payment_EnrollmentRequestId");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_CertificateRequestId",
                table: "Payment",
                newName: "IX_Payment_CertificateRequestId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payment",
                table: "Payment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Payment_PaymentId",
                table: "Invoices",
                column: "PaymentId",
                principalTable: "Payment",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_CertificateRequests_CertificateRequestId",
                table: "Payment",
                column: "CertificateRequestId",
                principalTable: "CertificateRequests",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_EnrollmentRequests_EnrollmentRequestId",
                table: "Payment",
                column: "EnrollmentRequestId",
                principalTable: "EnrollmentRequests",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Students_StudentNumber",
                table: "Payment",
                column: "StudentNumber",
                principalTable: "Students",
                principalColumn: "StudentNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_TranscriptRequests_TranscriptRequestId",
                table: "Payment",
                column: "TranscriptRequestId",
                principalTable: "TranscriptRequests",
                principalColumn: "Id");
        }
    }
}
