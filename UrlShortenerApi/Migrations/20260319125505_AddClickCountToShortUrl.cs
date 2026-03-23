using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrlShortenerApi.Migrations
{
    /// <inheritdoc />
    public partial class AddClickCountToShortUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClickCount",
                table: "ShortUrls",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClickCount",
                table: "ShortUrls");
        }
    }
}
