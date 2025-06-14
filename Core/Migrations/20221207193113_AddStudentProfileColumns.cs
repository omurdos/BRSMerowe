using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class AddStudentProfileColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CountryId",
                table: "Students",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ParentPhone",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StateId",
                table: "Students",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_CountryId",
                table: "Students",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_StateId",
                table: "Students",
                column: "StateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Countries_CountryId",
                table: "Students",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_States_StateId",
                table: "Students",
                column: "StateId",
                principalTable: "States",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Countries_CountryId",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_States_StateId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_CountryId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_StateId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "ParentPhone",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "StateId",
                table: "Students");
        }
    }
}
