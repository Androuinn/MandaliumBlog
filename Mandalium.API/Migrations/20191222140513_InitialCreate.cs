using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mandalium.API.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlogEntries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Headline = table.Column<string>(type: "varchar(200)", nullable: false),
                    SubHeadline = table.Column<string>(type: "varchar(500)", nullable: false),
                    innerTextHtml = table.Column<string>(type: "varchar(MAX)", nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogEntries", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogEntries");
        }
    }
}
