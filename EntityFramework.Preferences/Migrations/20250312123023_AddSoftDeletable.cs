using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EntityFramework.Preferences.Migrations
{
    /// <inheritdoc />
    public partial class AddSoftDeletable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "SingleSelectValue",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "SingleSelectValue",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Row",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Row",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Grid",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Grid",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Column",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Column",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "SingleSelectValue");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "SingleSelectValue");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Row");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Row");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Grid");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Grid");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Column");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Column");
        }
    }
}
