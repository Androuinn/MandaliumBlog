using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mandalium.Core.Migrations
{
    public partial class MostReadAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MostReadEntries",
                columns: table => new
                {
                    BlogEntryId = table.Column<int>(type: "int", nullable: false),
                    IsWriterEntry = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MostReadEntries");
        }
    }
}
