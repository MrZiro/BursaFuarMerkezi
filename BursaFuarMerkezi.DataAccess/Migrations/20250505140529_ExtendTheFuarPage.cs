using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BursaFuarMerkezi.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ExtendTheFuarPage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "FuarPages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "FairHall",
                table: "FuarPages",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FairLocation",
                table: "FuarPages",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Organizer",
                table: "FuarPages",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "FuarPages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "VisitingHours",
                table: "FuarPages",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WebsiteUrl",
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
                name: "EndDate",
                table: "FuarPages");

            migrationBuilder.DropColumn(
                name: "FairHall",
                table: "FuarPages");

            migrationBuilder.DropColumn(
                name: "FairLocation",
                table: "FuarPages");

            migrationBuilder.DropColumn(
                name: "Organizer",
                table: "FuarPages");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "FuarPages");

            migrationBuilder.DropColumn(
                name: "VisitingHours",
                table: "FuarPages");

            migrationBuilder.DropColumn(
                name: "WebsiteUrl",
                table: "FuarPages");
        }
    }
}
