using Microsoft.EntityFrameworkCore.Migrations;

namespace Mandalium.Core.Migrations
{
    public partial class MostReadSps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"CREATE OR ALTER PROCEDURE GetMostReadEntries
as
Begin

IF EXISTS
		(SELECT TOP (1) MostReadEntries.BlogEntryId
		FROM MostReadEntries where MostReadEntries.CreatedOn> DATEADD(DD,-7,Getdate()))

	BEGIN 
		SELECT * FROM BlogEntries WHERE Id in(
		SELECT TOP (5) MostReadEntries.BlogEntryId
		FROM MostReadEntries WHERE MostReadEntries.CreatedOn> DATEADD(DD,-7,Getdate()) 
		GROUP BY MostReadEntries.BlogEntryId ORDER BY COUNT(*) DESC)
	END
ELSE 
	BEGIN
		SELECT TOP 5 * FROM BlogEntries ORDER BY TimesRead DESC
	END
END
GO");
            migrationBuilder.Sql(
                @"CREATE OR ALTER PROCEDURE [dbo].[InsertMostRead] @BlogId int, @WriterEntry  bit
as
BEGIN
insert into MostReadEntries (BlogEntryId, IsWriterEntry, CreatedOn) values(@BlogId, @WriterEntry, GETDATE())
end 
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
