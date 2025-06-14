using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class removephotomanagement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanEditPersonalPhoto",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "IsPersonalPhotoApproved",
                table: "Students");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CanEditPersonalPhoto",
                table: "Students",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPersonalPhotoApproved",
                table: "Students",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
