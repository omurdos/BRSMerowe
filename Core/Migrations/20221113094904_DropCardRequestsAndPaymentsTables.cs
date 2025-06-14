using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class DropCardRequestsAndPaymentsTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Payments_PaymentId",
                table: "Invoices");

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

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payments",
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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payments",
                table: "Payments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Payments_PaymentId",
                table: "Invoices",
                column: "PaymentId",
                principalTable: "Payments",
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
    }
}
