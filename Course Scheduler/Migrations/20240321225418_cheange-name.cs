using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Course_Scheduler.Migrations
{
    /// <inheritdoc />
    public partial class cheangename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Courses_RequiredCourseID",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "RequiredCourseID",
                table: "Courses",
                newName: "PrerequisiteID");

            migrationBuilder.RenameIndex(
                name: "IX_Courses_RequiredCourseID",
                table: "Courses",
                newName: "IX_Courses_PrerequisiteID");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Courses_PrerequisiteID",
                table: "Courses",
                column: "PrerequisiteID",
                principalTable: "Courses",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Courses_PrerequisiteID",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "PrerequisiteID",
                table: "Courses",
                newName: "RequiredCourseID");

            migrationBuilder.RenameIndex(
                name: "IX_Courses_PrerequisiteID",
                table: "Courses",
                newName: "IX_Courses_RequiredCourseID");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Courses_RequiredCourseID",
                table: "Courses",
                column: "RequiredCourseID",
                principalTable: "Courses",
                principalColumn: "ID");
        }
    }
}
