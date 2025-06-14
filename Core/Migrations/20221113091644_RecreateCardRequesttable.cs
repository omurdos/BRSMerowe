using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class RecreateCardRequesttable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardRequest_RequestStatuses_StatusId",
                table: "CardRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_CardRequest_Services_ServiceId",
                table: "CardRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_CardRequest_Students_StudentNumber",
                table: "CardRequest");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_CardRequest_CardRequestId",
                table: "Payments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CardRequest",
                table: "CardRequest");

            migrationBuilder.RenameTable(
                name: "CardRequest",
                newName: "CardRequests");

            migrationBuilder.RenameIndex(
                name: "IX_CardRequest_StudentNumber",
                table: "CardRequests",
                newName: "IX_CardRequests_StudentNumber");

            migrationBuilder.RenameIndex(
                name: "IX_CardRequest_StatusId",
                table: "CardRequests",
                newName: "IX_CardRequests_StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_CardRequest_ServiceId",
                table: "CardRequests",
                newName: "IX_CardRequests_ServiceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CardRequests",
                table: "CardRequests",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CardRequests_RequestStatuses_StatusId",
                table: "CardRequests",
                column: "StatusId",
                principalTable: "RequestStatuses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CardRequests_Services_ServiceId",
                table: "CardRequests",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CardRequests_Students_StudentNumber",
                table: "CardRequests",
                column: "StudentNumber",
                principalTable: "Students",
                principalColumn: "StudentNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_CardRequests_CardRequestId",
                table: "Payments",
                column: "CardRequestId",
                principalTable: "CardRequests",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardRequests_RequestStatuses_StatusId",
                table: "CardRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_CardRequests_Services_ServiceId",
                table: "CardRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_CardRequests_Students_StudentNumber",
                table: "CardRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_CardRequests_CardRequestId",
                table: "Payments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CardRequests",
                table: "CardRequests");

            migrationBuilder.RenameTable(
                name: "CardRequests",
                newName: "CardRequest");

            migrationBuilder.RenameIndex(
                name: "IX_CardRequests_StudentNumber",
                table: "CardRequest",
                newName: "IX_CardRequest_StudentNumber");

            migrationBuilder.RenameIndex(
                name: "IX_CardRequests_StatusId",
                table: "CardRequest",
                newName: "IX_CardRequest_StatusId");

            migrationBuilder.RenameIndex(
                name: "IX_CardRequests_ServiceId",
                table: "CardRequest",
                newName: "IX_CardRequest_ServiceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CardRequest",
                table: "CardRequest",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CardRequest_RequestStatuses_StatusId",
                table: "CardRequest",
                column: "StatusId",
                principalTable: "RequestStatuses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CardRequest_Services_ServiceId",
                table: "CardRequest",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CardRequest_Students_StudentNumber",
                table: "CardRequest",
                column: "StudentNumber",
                principalTable: "Students",
                principalColumn: "StudentNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_CardRequest_CardRequestId",
                table: "Payments",
                column: "CardRequestId",
                principalTable: "CardRequest",
                principalColumn: "Id");
        }
    }
}
