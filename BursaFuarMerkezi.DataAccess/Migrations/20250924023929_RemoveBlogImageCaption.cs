using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BursaFuarMerkezi.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemoveBlogImageCaption : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Caption",
                table: "BlogImages");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Caption",
                table: "BlogImages",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);
        }
    }
}
