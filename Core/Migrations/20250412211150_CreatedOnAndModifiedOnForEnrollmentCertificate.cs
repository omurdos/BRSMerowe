using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class CreatedOnAndModifiedOnForEnrollmentCertificate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.AddColumn<decimal>(
            //     name: "SubjectCodeId",
            //     table: "Subjects",
            //     type: "decimal(18,2)",
            //     nullable: false,
            //     defaultValue: 0m);

            // migrationBuilder.AddColumn<decimal>(
            //     name: "SubjectCodeId",
            //     table: "StudentSubjects",
            //     type: "decimal(18,2)",
            //     nullable: false,
            //     defaultValue: 0m);

            // migrationBuilder.AddColumn<int>(
            //     name: "ViewYesNO",
            //     table: "StudentSubjects",
            //     type: "int",
            //     nullable: false,
            //     defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "EnrollmentRequests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "EnrollmentRequests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropColumn(
            //     name: "SubjectCodeId",
            //     table: "Subjects");

            // migrationBuilder.DropColumn(
            //     name: "SubjectCodeId",
            //     table: "StudentSubjects");

            // migrationBuilder.DropColumn(
            //     name: "ViewYesNO",
            //     table: "StudentSubjects");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "EnrollmentRequests");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "EnrollmentRequests");
        }
    }
}
