using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DriverGuide.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveNavigationProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionAnswers_Questions",
                table: "QuestionAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionAnswers_TestSessions",
                table: "QuestionAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_TestSessions_AspNetUsers",
                table: "TestSessions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_QuestionAnswers_Questions",
                table: "QuestionAnswers",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "QuestionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionAnswers_TestSessions",
                table: "QuestionAnswers",
                column: "TestSessionId",
                principalTable: "TestSessions",
                principalColumn: "TestSessionId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TestSessions_AspNetUsers",
                table: "TestSessions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
