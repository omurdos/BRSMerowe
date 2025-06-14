using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class AddResolutionAudittoTicket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_TicketStatuses_TicketStatusId",
                table: "Tickets");

            migrationBuilder.DropTable(
                name: "TicketStatuses");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_TicketStatusId",
                table: "Tickets");

            migrationBuilder.RenameColumn(
                name: "TicketStatusId",
                table: "Tickets",
                newName: "TicketStatus");

            migrationBuilder.AddColumn<DateTime>(
                name: "ClosedOn",
                table: "Tickets",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResolvedBy",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResolvedOn",
                table: "Tickets",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClosedOn",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "ResolvedBy",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "ResolvedOn",
                table: "Tickets");

            migrationBuilder.RenameColumn(
                name: "TicketStatus",
                table: "Tickets",
                newName: "TicketStatusId");

            migrationBuilder.CreateTable(
                name: "TicketStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusNameA = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusNameE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketStatuses", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_TicketStatusId",
                table: "Tickets",
                column: "TicketStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_TicketStatuses_TicketStatusId",
                table: "Tickets",
                column: "TicketStatusId",
                principalTable: "TicketStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
