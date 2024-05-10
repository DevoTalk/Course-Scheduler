using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Course_Scheduler.Migrations
{
    /// <inheritdoc />
    public partial class addSemester : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Courses_PrerequisiteID",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_PrerequisiteID",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "PrerequisiteID",
                table: "Courses");

            migrationBuilder.CreateTable(
                name: "Semester",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Semester", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "CourseToSemester",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CourseID = table.Column<int>(type: "INTEGER", nullable: false),
                    SemesterID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseToSemester", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CourseToSemester_Courses_CourseID",
                        column: x => x.CourseID,
                        principalTable: "Courses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseToSemester_Semester_SemesterID",
                        column: x => x.SemesterID,
                        principalTable: "Semester",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseToSemester_CourseID",
                table: "CourseToSemester",
                column: "CourseID");

            migrationBuilder.CreateIndex(
                name: "IX_CourseToSemester_SemesterID",
                table: "CourseToSemester",
                column: "SemesterID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseToSemester");

            migrationBuilder.DropTable(
                name: "Semester");

            migrationBuilder.AddColumn<int>(
                name: "PrerequisiteID",
                table: "Courses",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Courses_PrerequisiteID",
                table: "Courses",
                column: "PrerequisiteID");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Courses_PrerequisiteID",
                table: "Courses",
                column: "PrerequisiteID",
                principalTable: "Courses",
                principalColumn: "ID");
        }
    }
}
