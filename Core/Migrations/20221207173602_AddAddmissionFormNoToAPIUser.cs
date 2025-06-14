using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class AddAddmissionFormNoToAPIUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
               name: "AddmissionFormNo",
               table: "AspNetUsers",
               type: "nvarchar(max)",
               nullable: true);
            return;

            migrationBuilder.DropForeignKey(
                name: "FK_StudentSubjects_Subjects",
                table: "StudentSubjects");

            migrationBuilder.DropForeignKey(
                name: "FK_TblSections_Subjects",
                table: "TblSections");

            migrationBuilder.DropIndex(
                name: "IX_TblSections_SubjectCodeID",
                table: "TblSections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subjects",
                table: "Subjects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentSubjects",
                table: "StudentSubjects");

            migrationBuilder.DropIndex(
                name: "IX_StudentSubjects_SubjectCodeID",
                table: "StudentSubjects");

            migrationBuilder.DropColumn(
                name: "SubjectCodeID",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "OrederInResult",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "required",
                table: "Subjects");

            migrationBuilder.DropColumn(
                name: "SubjectCodeID",
                table: "StudentSubjects");

            migrationBuilder.DropColumn(
                name: "RegistrationType",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "BatchID",
                table: "Gradepoints");

            migrationBuilder.DropColumn(
                name: "pcgpa",
                table: "Gradepoints");

            migrationBuilder.DropColumn(
                name: "ppcgpa",
                table: "Gradepoints");

            migrationBuilder.RenameColumn(
                name: "SubjectCodeID",
                table: "TblSections",
                newName: "SubjectCodeId");

            migrationBuilder.AlterColumn<decimal>(
                name: "SubjectCodeId",
                table: "TblSections",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "OrederInResult",
                table: "Students",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)",
                oldNullable: true,
                oldDefaultValueSql: "((0))");

            migrationBuilder.AlterColumn<string>(
                name: "BatchID",
                table: "Students",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "cgpa",
                table: "Gradepoints",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BatchID",
                table: "batches",
                type: "nvarchar(50)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)")
                .OldAnnotation("SqlServer:Identity", "1, 1");

           

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subjects",
                table: "Subjects",
                column: "SubjectCode");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentSubjects",
                table: "StudentSubjects",
                columns: new[] { "StudentNumber", "SubjectCode" });

            migrationBuilder.CreateIndex(
                name: "IX_StudentSubjects_SubjectCode",
                table: "StudentSubjects",
                column: "SubjectCode");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentSubjects_Subjects",
                table: "StudentSubjects",
                column: "SubjectCode",
                principalTable: "Subjects",
                principalColumn: "SubjectCode",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TblSections_Subjects",
                table: "TblSections",
                column: "SubjectCode",
                principalTable: "Subjects",
                principalColumn: "SubjectCode",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentSubjects_Subjects",
                table: "StudentSubjects");

            migrationBuilder.DropForeignKey(
                name: "FK_TblSections_Subjects",
                table: "TblSections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subjects",
                table: "Subjects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StudentSubjects",
                table: "StudentSubjects");

            migrationBuilder.DropIndex(
                name: "IX_StudentSubjects_SubjectCode",
                table: "StudentSubjects");

            migrationBuilder.DropColumn(
                name: "AddmissionFormNo",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "SubjectCodeId",
                table: "TblSections",
                newName: "SubjectCodeID");

            migrationBuilder.AlterColumn<decimal>(
                name: "SubjectCodeID",
                table: "TblSections",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<decimal>(
                name: "SubjectCodeID",
                table: "Subjects",
                type: "numeric(18,0)",
                nullable: false,
                defaultValue: 0m)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<decimal>(
                name: "OrederInResult",
                table: "Subjects",
                type: "numeric(18,0)",
                nullable: true,
                defaultValueSql: "((0))");

            migrationBuilder.AddColumn<int>(
                name: "required",
                table: "Subjects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "SubjectCodeID",
                table: "StudentSubjects",
                type: "numeric(18,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "OrederInResult",
                table: "Students",
                type: "numeric(18,0)",
                nullable: true,
                defaultValueSql: "((0))",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "BatchID",
                table: "Students",
                type: "numeric(18,0)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RegistrationType",
                table: "Students",
                type: "int",
                nullable: true,
                defaultValueSql: "((0))");

            migrationBuilder.AlterColumn<float>(
                name: "cgpa",
                table: "Gradepoints",
                type: "real",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<decimal>(
                name: "BatchID",
                table: "Gradepoints",
                type: "numeric(18,0)",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "pcgpa",
                table: "Gradepoints",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "ppcgpa",
                table: "Gradepoints",
                type: "real",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "BatchID",
                table: "batches",
                type: "numeric(18,0)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subjects",
                table: "Subjects",
                column: "SubjectCodeID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StudentSubjects",
                table: "StudentSubjects",
                columns: new[] { "StudentNumber", "SubjectCode", "SubjectCodeID" });

            migrationBuilder.CreateIndex(
                name: "IX_TblSections_SubjectCodeID",
                table: "TblSections",
                column: "SubjectCodeID");

            migrationBuilder.CreateIndex(
                name: "IX_StudentSubjects_SubjectCodeID",
                table: "StudentSubjects",
                column: "SubjectCodeID");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentSubjects_Subjects",
                table: "StudentSubjects",
                column: "SubjectCodeID",
                principalTable: "Subjects",
                principalColumn: "SubjectCodeID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TblSections_Subjects",
                table: "TblSections",
                column: "SubjectCodeID",
                principalTable: "Subjects",
                principalColumn: "SubjectCodeID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
