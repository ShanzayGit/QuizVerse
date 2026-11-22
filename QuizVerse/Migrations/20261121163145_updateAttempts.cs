using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizVerse.Migrations
{
    /// <inheritdoc />
    public partial class updateAttempts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attempts_Quizzes_QuizId",
                table: "Attempts");

            migrationBuilder.AlterColumn<int>(
                name: "QuizId",
                table: "Attempts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Attempts_Quizzes_QuizId",
                table: "Attempts",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attempts_Quizzes_QuizId",
                table: "Attempts");

            migrationBuilder.AlterColumn<int>(
                name: "QuizId",
                table: "Attempts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Attempts_Quizzes_QuizId",
                table: "Attempts",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "Id");
        }
    }
}
