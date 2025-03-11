using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EntityFramework.Preferences.Migrations
{
    /// <inheritdoc />
    public partial class AddSingleSelectValueTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Values",
                table: "Column");

            migrationBuilder.CreateTable(
                name: "SingleSelectValue",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Value = table.Column<string>(type: "text", nullable: false),
                    SingleSelectColumnId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SingleSelectValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SingleSelectValue_Column_SingleSelectColumnId",
                        column: x => x.SingleSelectColumnId,
                        principalTable: "Column",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SingleSelectValue_SingleSelectColumnId",
                table: "SingleSelectValue",
                column: "SingleSelectColumnId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SingleSelectValue");

            migrationBuilder.AddColumn<List<string>>(
                name: "Values",
                table: "Column",
                type: "jsonb",
                nullable: true);
        }
    }
}
