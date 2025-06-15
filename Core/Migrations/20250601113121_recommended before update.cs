using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class recommendedbeforeupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropIndex(
            //     name: "IX_Payments_CertificateRequestId",
            //     table: "Payments");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CertificateRequestId",
                table: "Payments",
                column: "CertificateRequestId",
                unique: true,
                filter: "[CertificateRequestId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Payments_CertificateRequestId",
                table: "Payments");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CertificateRequestId",
                table: "Payments",
                column: "CertificateRequestId");
        }
    }
}
