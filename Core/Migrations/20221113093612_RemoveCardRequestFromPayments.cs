using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class RemoveCardRequestFromPayments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_CardRequests_CardRequestId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_CardRequestId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "CardRequestId",
                table: "Payments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CardRequestId",
                table: "Payments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CardRequestId",
                table: "Payments",
                column: "CardRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_CardRequests_CardRequestId",
                table: "Payments",
                column: "CardRequestId",
                principalTable: "CardRequests",
                principalColumn: "Id");
        }
    }
}
