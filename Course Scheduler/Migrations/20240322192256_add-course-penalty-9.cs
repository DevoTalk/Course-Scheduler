using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Course_Scheduler.Migrations
{
    /// <inheritdoc />
    public partial class addcoursepenalty9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "asdfasdfadsf",
                table: "CoursePenalty",
                newName: "RelatedID");

            migrationBuilder.RenameColumn(
                name: "RelatedCourasdfasdfasdfasdfse",
                table: "CoursePenalty",
                newName: "CourseID");

            migrationBuilder.CreateIndex(
                name: "IX_CoursePenalty_CourseID",
                table: "CoursePenalty",
                column: "CourseID");

            migrationBuilder.AddForeignKey(
                name: "FK_CoursePenalty_Courses_CourseID",
                table: "CoursePenalty",
                column: "CourseID",
                principalTable: "Courses",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoursePenalty_Courses_CourseID",
                table: "CoursePenalty");

            migrationBuilder.DropIndex(
                name: "IX_CoursePenalty_CourseID",
                table: "CoursePenalty");

            migrationBuilder.RenameColumn(
                name: "RelatedID",
                table: "CoursePenalty",
                newName: "asdfasdfadsf");

            migrationBuilder.RenameColumn(
                name: "CourseID",
                table: "CoursePenalty",
                newName: "RelatedCourasdfasdfasdfasdfse");
        }
    }
}
