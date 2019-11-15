using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Examportal.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    email = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    name = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
                    password = table.Column<string>(unicode: false, maxLength: 200, nullable: true),
                    accountType = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
                    collegeName = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
                    phoneNumber = table.Column<string>(unicode: false, maxLength: 10, nullable: true),
                    createdDate = table.Column<DateTime>(type: "date", nullable: true),
                    createdBy = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
                    modifiedDate = table.Column<DateTime>(type: "date", nullable: true),
                    modifiedBy = table.Column<string>(unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.email);
                });

            migrationBuilder.CreateTable(
                name: "examDetails",
                columns: table => new
                {
                    _id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    examName = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
                    examCode = table.Column<string>(unicode: false, maxLength: 20, nullable: false),
                    email = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    examDuration = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
                    examStartTime = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
                    createdDate = table.Column<DateTime>(type: "date", nullable: true),
                    createdBy = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
                    modifiedDate = table.Column<DateTime>(type: "date", nullable: true),
                    modifiedBy = table.Column<string>(unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_examDetails", x => x.examCode);
                    table.ForeignKey(
                        name: "FK__examDetai__email__66603565",
                        column: x => x.email,
                        principalTable: "users",
                        principalColumn: "email",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "questions",
                columns: table => new
                {
                    _id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    questionText = table.Column<string>(unicode: false, maxLength: 200, nullable: true),
                    answer = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    answerType = table.Column<string>(unicode: false, maxLength: 40, nullable: true),
                    option1 = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    option2 = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    option3 = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    option4 = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    weightage = table.Column<int>(nullable: true),
                    examCode = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
                    questionImage = table.Column<string>(unicode: false, maxLength: 100, nullable: true),
                    createdDate = table.Column<DateTime>(type: "date", nullable: true),
                    createdBy = table.Column<string>(unicode: false, maxLength: 40, nullable: true),
                    modifiedDate = table.Column<DateTime>(type: "date", nullable: true),
                    modifiedBy = table.Column<string>(unicode: false, maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_questions", x => x._id);
                    table.ForeignKey(
                        name: "FK__questions__examC__693CA210",
                        column: x => x.examCode,
                        principalTable: "examDetails",
                        principalColumn: "examCode",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "candidateAnswer",
                columns: table => new
                {
                    candidateId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    email = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    totalScore = table.Column<int>(nullable: true),
                    testCode = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
                    completionTime = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
                    submitExam = table.Column<byte>(nullable: true),
                    correctStatus = table.Column<byte>(nullable: true),
                    _id = table.Column<int>(nullable: true),
                    createdDate = table.Column<DateTime>(type: "date", nullable: true),
                    createdBy = table.Column<string>(unicode: false, maxLength: 20, nullable: true),
                    modifiedDate = table.Column<DateTime>(type: "date", nullable: true),
                    modifiedBy = table.Column<string>(unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_candidateAnswer", x => x.candidateId);
                    table.ForeignKey(
                        name: "FK__candidate__email__73BA3083",
                        column: x => x.email,
                        principalTable: "users",
                        principalColumn: "email",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__candidateAn___id__72C60C4A",
                        column: x => x._id,
                        principalTable: "questions",
                        principalColumn: "_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_candidateAnswer_email",
                table: "candidateAnswer",
                column: "email");

            migrationBuilder.CreateIndex(
                name: "IX_candidateAnswer__id",
                table: "candidateAnswer",
                column: "_id");

            migrationBuilder.CreateIndex(
                name: "IX_examDetails_email",
                table: "examDetails",
                column: "email");

            migrationBuilder.CreateIndex(
                name: "IX_questions_examCode",
                table: "questions",
                column: "examCode");

            migrationBuilder.CreateIndex(
                name: "UQ__users__4849DA013426D3A4",
                table: "users",
                column: "phoneNumber",
                unique: true,
                filter: "[phoneNumber] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "candidateAnswer");

            migrationBuilder.DropTable(
                name: "questions");

            migrationBuilder.DropTable(
                name: "examDetails");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
