using Microsoft.EntityFrameworkCore.Migrations;

namespace Examportal.Migrations
{
    public partial class updatedModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__candidate__email__73BA3083",
                table: "candidateAnswer");

            migrationBuilder.DropForeignKey(
                name: "FK__candidateAn___id__72C60C4A",
                table: "candidateAnswer");

            migrationBuilder.DropIndex(
                name: "IX_candidateAnswer_email",
                table: "candidateAnswer");

            migrationBuilder.DropColumn(
                name: "submitExam",
                table: "candidateAnswer");

            migrationBuilder.DropColumn(
                name: "totalScore",
                table: "candidateAnswer");

            migrationBuilder.CreateTable(
                name: "candidateResult",
                columns: table => new
                {
                    email = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    totalScore = table.Column<int>(nullable: true),
                    testCode = table.Column<string>(unicode: false, maxLength: 20, nullable: false),
                    submitExam = table.Column<byte>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_candidateResult", x => x.testCode);
                });

            migrationBuilder.CreateIndex(
                name: "IX_candidateAnswer_testCode",
                table: "candidateAnswer",
                column: "testCode");

            migrationBuilder.AddForeignKey(
                name: "FK__candidateAn___id__06CD04F7",
                table: "candidateAnswer",
                column: "_id",
                principalTable: "questions",
                principalColumn: "_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK__candidate__testC__07C12930",
                table: "candidateAnswer",
                column: "testCode",
                principalTable: "candidateResult",
                principalColumn: "testCode",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__candidateAn___id__06CD04F7",
                table: "candidateAnswer");

            migrationBuilder.DropForeignKey(
                name: "FK__candidate__testC__07C12930",
                table: "candidateAnswer");

            migrationBuilder.DropTable(
                name: "candidateResult");

            migrationBuilder.DropIndex(
                name: "IX_candidateAnswer_testCode",
                table: "candidateAnswer");

            migrationBuilder.AddColumn<byte>(
                name: "submitExam",
                table: "candidateAnswer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "totalScore",
                table: "candidateAnswer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_candidateAnswer_email",
                table: "candidateAnswer",
                column: "email");

            migrationBuilder.AddForeignKey(
                name: "FK__candidate__email__73BA3083",
                table: "candidateAnswer",
                column: "email",
                principalTable: "users",
                principalColumn: "email",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK__candidateAn___id__72C60C4A",
                table: "candidateAnswer",
                column: "_id",
                principalTable: "questions",
                principalColumn: "_id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
