using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DriverGuide.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeQuestionIdToInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "QuestionId",
                table: "QuestionAnswers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAnswers_QuestionId",
                table: "QuestionAnswers",
                column: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionAnswers_Questions",
                table: "QuestionAnswers",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "QuestionId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionAnswers_Questions",
                table: "QuestionAnswers");

            migrationBuilder.DropIndex(
                name: "IX_QuestionAnswers_QuestionId",
                table: "QuestionAnswers");

            migrationBuilder.AlterColumn<string>(
                name: "QuestionId",
                table: "QuestionAnswers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
