using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class studentid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Students_StudentNumber1",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "StudentNumber1",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "StudentId",
                table: "AspNetUsers",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Students_StudentNumber1",
                table: "AspNetUsers",
                column: "StudentNumber1",
                principalTable: "Students",
                principalColumn: "StudentNumber",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Students_StudentNumber1",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "StudentNumber1",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Students_StudentNumber1",
                table: "AspNetUsers",
                column: "StudentNumber1",
                principalTable: "Students",
                principalColumn: "StudentNumber");
        }
    }
}
