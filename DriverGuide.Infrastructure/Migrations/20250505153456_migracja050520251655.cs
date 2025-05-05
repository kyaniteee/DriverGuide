using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DriverGuide.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class migracja050520251655 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "QuestionFiles",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "FileMimeType",
                table: "QuestionFiles",
                newName: "ContentType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "QuestionFiles",
                newName: "FileName");

            migrationBuilder.RenameColumn(
                name: "ContentType",
                table: "QuestionFiles",
                newName: "FileMimeType");
        }
    }
}
