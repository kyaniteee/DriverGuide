using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DriverGuide.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexesToQuestionsAndQuestionFiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Media",
                table: "Questions",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "QuestionFiles",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_Media",
                table: "Questions",
                column: "Media");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionFiles_Name",
                table: "QuestionFiles",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Questions_Media",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_QuestionFiles_Name",
                table: "QuestionFiles");

            migrationBuilder.AlterColumn<string>(
                name: "Media",
                table: "Questions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "QuestionFiles",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
