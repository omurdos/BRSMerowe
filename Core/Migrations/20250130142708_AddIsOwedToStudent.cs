using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class AddIsOwedToStudent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Students_batches",
            //    table: "Students");

            //migrationBuilder.AlterColumn<decimal>(
            //    name: "BatchID",
            //    table: "Students",
            //    type: "numeric(18,0)",
            //    nullable: false,
            //    defaultValue: 0m,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(50)",
            //    oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsOwed",
                table: "Students",
                type: "bit",
                nullable: false,
                defaultValue: false);

            //migrationBuilder.AlterColumn<decimal>(
            //    name: "BatchID",
            //    table: "batches",
            //    type: "numeric(18,0)",
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldType: "nvarchar(50)")
            //    .Annotation("SqlServer:Identity", "1, 1");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Students_batches",
            //    table: "Students",
            //    column: "BatchID",
            //    principalTable: "batches",
            //    principalColumn: "BatchID",
            //    onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_batches",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "IsOwed",
                table: "Students");

            migrationBuilder.AlterColumn<string>(
                name: "BatchID",
                table: "Students",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)");

            migrationBuilder.AlterColumn<string>(
                name: "BatchID",
                table: "batches",
                type: "nvarchar(50)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,0)")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_batches",
                table: "Students",
                column: "BatchID",
                principalTable: "batches",
                principalColumn: "BatchID");
        }
    }
}
