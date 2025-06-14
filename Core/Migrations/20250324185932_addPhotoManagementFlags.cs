using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class addPhotoManagementFlags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CanEditPersonalPhoto",
                table: "Students",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPersonalPhotoApproved",
                table: "Students",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanEditPersonalPhoto",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "IsPersonalPhotoApproved",
                table: "Students");
        }
    }
}
