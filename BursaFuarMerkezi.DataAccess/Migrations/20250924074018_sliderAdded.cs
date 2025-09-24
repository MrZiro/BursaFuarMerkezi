using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BursaFuarMerkezi.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class sliderAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Sliders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AltText",
                table: "Sliders",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ButtonTextEn",
                table: "Sliders",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ButtonTextTr",
                table: "Sliders",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Sliders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DescriptionEn",
                table: "Sliders",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionTr",
                table: "Sliders",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkUrl",
                table: "Sliders",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MobileImageUrl",
                table: "Sliders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "OpenInNewTab",
                table: "Sliders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TitleEn",
                table: "Sliders",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TitleTr",
                table: "Sliders",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Sliders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AltText",
                table: "Sliders");

            migrationBuilder.DropColumn(
                name: "ButtonTextEn",
                table: "Sliders");

            migrationBuilder.DropColumn(
                name: "ButtonTextTr",
                table: "Sliders");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Sliders");

            migrationBuilder.DropColumn(
                name: "DescriptionEn",
                table: "Sliders");

            migrationBuilder.DropColumn(
                name: "DescriptionTr",
                table: "Sliders");

            migrationBuilder.DropColumn(
                name: "LinkUrl",
                table: "Sliders");

            migrationBuilder.DropColumn(
                name: "MobileImageUrl",
                table: "Sliders");

            migrationBuilder.DropColumn(
                name: "OpenInNewTab",
                table: "Sliders");

            migrationBuilder.DropColumn(
                name: "TitleEn",
                table: "Sliders");

            migrationBuilder.DropColumn(
                name: "TitleTr",
                table: "Sliders");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Sliders");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Sliders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
