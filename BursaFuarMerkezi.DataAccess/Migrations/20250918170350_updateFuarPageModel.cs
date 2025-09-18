using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BursaFuarMerkezi.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updateFuarPageModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "FuarPages",
                newName: "TitleTr");

            migrationBuilder.RenameColumn(
                name: "SubTitle",
                table: "FuarPages",
                newName: "TitleEn");

            migrationBuilder.RenameColumn(
                name: "Slug",
                table: "FuarPages",
                newName: "SubTitleTr");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "FuarPages",
                newName: "ContentTr");

            migrationBuilder.AddColumn<string>(
                name: "ContentEn",
                table: "FuarPages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SlugEn",
                table: "FuarPages",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SlugTr",
                table: "FuarPages",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubTitleEn",
                table: "FuarPages",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_FuarPages_SlugEn",
                table: "FuarPages",
                column: "SlugEn",
                unique: true,
                filter: "[SlugEn] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FuarPages_SlugTr",
                table: "FuarPages",
                column: "SlugTr",
                unique: true,
                filter: "[SlugTr] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FuarPages_SlugEn",
                table: "FuarPages");

            migrationBuilder.DropIndex(
                name: "IX_FuarPages_SlugTr",
                table: "FuarPages");

            migrationBuilder.DropColumn(
                name: "ContentEn",
                table: "FuarPages");

            migrationBuilder.DropColumn(
                name: "SlugEn",
                table: "FuarPages");

            migrationBuilder.DropColumn(
                name: "SlugTr",
                table: "FuarPages");

            migrationBuilder.DropColumn(
                name: "SubTitleEn",
                table: "FuarPages");

            migrationBuilder.RenameColumn(
                name: "TitleTr",
                table: "FuarPages",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "TitleEn",
                table: "FuarPages",
                newName: "SubTitle");

            migrationBuilder.RenameColumn(
                name: "SubTitleTr",
                table: "FuarPages",
                newName: "Slug");

            migrationBuilder.RenameColumn(
                name: "ContentTr",
                table: "FuarPages",
                newName: "Content");
        }
    }
}
