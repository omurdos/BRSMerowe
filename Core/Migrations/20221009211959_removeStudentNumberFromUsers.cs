using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class removeStudentNumberFromUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Students_StudentNumber1",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_StudentNumber1",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "StudentNumber1",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "StudentNumber",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_StudentNumber",
                table: "AspNetUsers",
                column: "StudentNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Students_StudentNumber",
                table: "AspNetUsers",
                column: "StudentNumber",
                principalTable: "Students",
                principalColumn: "StudentNumber",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Students_StudentNumber",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_StudentNumber",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "StudentNumber",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)");

            migrationBuilder.AddColumn<string>(
                name: "StudentNumber1",
                table: "AspNetUsers",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_StudentNumber1",
                table: "AspNetUsers",
                column: "StudentNumber1");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Students_StudentNumber1",
                table: "AspNetUsers",
                column: "StudentNumber1",
                principalTable: "Students",
                principalColumn: "StudentNumber",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
