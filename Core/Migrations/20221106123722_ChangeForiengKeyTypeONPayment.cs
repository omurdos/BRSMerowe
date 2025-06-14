using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class ChangeForiengKeyTypeONPayment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_CardRequests_CardRequestId1",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_CardRequestId1",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "CardRequestId1",
                table: "Payments");

            migrationBuilder.AlterColumn<long>(
                name: "CardRequestId",
                table: "Payments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CardRequestId",
                table: "Payments",
                column: "CardRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_CardRequests_CardRequestId",
                table: "Payments",
                column: "CardRequestId",
                principalTable: "CardRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_CardRequests_CardRequestId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_CardRequestId",
                table: "Payments");

            migrationBuilder.AlterColumn<string>(
                name: "CardRequestId",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "CardRequestId1",
                table: "Payments",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CardRequestId1",
                table: "Payments",
                column: "CardRequestId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_CardRequests_CardRequestId1",
                table: "Payments",
                column: "CardRequestId1",
                principalTable: "CardRequests",
                principalColumn: "Id");
        }
    }
}
