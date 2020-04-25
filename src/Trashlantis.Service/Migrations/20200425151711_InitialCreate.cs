using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Trashlantis.Service.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrashRemovalState",
                columns: table => new
                {
                    CorrelationId = table.Column<Guid>(nullable: false),
                    CurrentState = table.Column<int>(nullable: false),
                    BinNumber = table.Column<string>(nullable: true),
                    RequestTimestamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrashRemovalState", x => x.CorrelationId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TrashRemovalState_BinNumber",
                table: "TrashRemovalState",
                column: "BinNumber",
                unique: true,
                filter: "[BinNumber] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrashRemovalState");
        }
    }
}
