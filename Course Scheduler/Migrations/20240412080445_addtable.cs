using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Course_Scheduler.Migrations
{
    /// <inheritdoc />
    public partial class addtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CourseTeacherClassTime",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CourseID = table.Column<int>(type: "INTEGER", nullable: false),
                    TeacherID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseTeacherClassTime", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CourseTeacherClassTime_Courses_CourseID",
                        column: x => x.CourseID,
                        principalTable: "Courses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseTeacherClassTime_Teacher_TeacherID",
                        column: x => x.TeacherID,
                        principalTable: "Teacher",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EvenOddClassTime",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ClassTime = table.Column<int>(type: "INTEGER", nullable: false),
                    EvenOdd = table.Column<int>(type: "INTEGER", nullable: true),
                    CourseTeacherClassTimeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EvenOddClassTime", x => x.ID);
                    table.ForeignKey(
                        name: "FK_EvenOddClassTime_CourseTeacherClassTime_CourseTeacherClassTimeId",
                        column: x => x.CourseTeacherClassTimeId,
                        principalTable: "CourseTeacherClassTime",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseTeacherClassTime_CourseID",
                table: "CourseTeacherClassTime",
                column: "CourseID");

            migrationBuilder.CreateIndex(
                name: "IX_CourseTeacherClassTime_TeacherID",
                table: "CourseTeacherClassTime",
                column: "TeacherID");

            migrationBuilder.CreateIndex(
                name: "IX_EvenOddClassTime_CourseTeacherClassTimeId",
                table: "EvenOddClassTime",
                column: "CourseTeacherClassTimeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EvenOddClassTime");

            migrationBuilder.DropTable(
                name: "CourseTeacherClassTime");
        }
    }
}
