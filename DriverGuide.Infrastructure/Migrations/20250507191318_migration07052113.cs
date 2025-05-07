using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DriverGuide.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class migration07052113 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsGeneral",
                table: "Questions",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<int>(
                name: "Points",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "TimeToAnswerSeconds",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 30);

            migrationBuilder.AlterColumn<DateOnly>(
                name: "UploadDate",
                table: "QuestionFiles",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(2025, 5, 7),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldDefaultValue: new DateOnly(2025, 5, 5));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsGeneral",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "Points",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "TimeToAnswerSeconds",
                table: "Questions");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "UploadDate",
                table: "QuestionFiles",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(2025, 5, 5),
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldDefaultValue: new DateOnly(2025, 5, 7));
        }
    }
}
