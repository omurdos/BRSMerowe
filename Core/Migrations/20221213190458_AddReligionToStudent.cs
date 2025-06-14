using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class AddReligionToStudent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReligionId",
                table: "Students",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_ReligionId",
                table: "Students",
                column: "ReligionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Religions_ReligionId",
                table: "Students",
                column: "ReligionId",
                principalTable: "Religions",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Religions_ReligionId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_ReligionId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "ReligionId",
                table: "Students");
        }
    }
}
