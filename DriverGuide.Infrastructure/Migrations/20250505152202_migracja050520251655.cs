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
                name: "DataDodania",
                table: "QuestionFiles",
                newName: "UploadDate");

            migrationBuilder.AddColumn<string>(
                name: "FileMimeType",
                table: "QuestionFiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileMimeType",
                table: "QuestionFiles");

            migrationBuilder.RenameColumn(
                name: "UploadDate",
                table: "QuestionFiles",
                newName: "DataDodania");
        }
    }
}
