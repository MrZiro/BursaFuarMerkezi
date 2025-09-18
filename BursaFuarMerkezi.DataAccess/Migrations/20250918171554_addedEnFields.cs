using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BursaFuarMerkezi.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addedEnFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Organizer",
                table: "FuarPages",
                newName: "OrganizerTr");

            migrationBuilder.RenameColumn(
                name: "FairLocation",
                table: "FuarPages",
                newName: "FairLocationTr");

            migrationBuilder.RenameColumn(
                name: "FairCategory",
                table: "FuarPages",
                newName: "OrganizerEn");

            migrationBuilder.AddColumn<string>(
                name: "FairCategoryEn",
                table: "FuarPages",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FairCategoryTr",
                table: "FuarPages",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FairLocationEn",
                table: "FuarPages",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FairCategoryEn",
                table: "FuarPages");

            migrationBuilder.DropColumn(
                name: "FairCategoryTr",
                table: "FuarPages");

            migrationBuilder.DropColumn(
                name: "FairLocationEn",
                table: "FuarPages");

            migrationBuilder.RenameColumn(
                name: "OrganizerTr",
                table: "FuarPages",
                newName: "Organizer");

            migrationBuilder.RenameColumn(
                name: "OrganizerEn",
                table: "FuarPages",
                newName: "FairCategory");

            migrationBuilder.RenameColumn(
                name: "FairLocationTr",
                table: "FuarPages",
                newName: "FairLocation");
        }
    }
}
