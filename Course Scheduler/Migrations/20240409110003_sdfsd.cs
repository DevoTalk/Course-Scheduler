using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Course_Scheduler.Migrations
{
    /// <inheritdoc />
    public partial class sdfsd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PreferredTime",
                table: "Teacher");

            migrationBuilder.CreateTable(
                name: "TeacherClassTimeWithPenalties",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TeacherId = table.Column<int>(type: "INTEGER", nullable: false),
                    PreferredTime = table.Column<int>(type: "INTEGER", nullable: false),
                    Penalty = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherClassTimeWithPenalties", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TeacherClassTimeWithPenalties_Teacher_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teacher",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeacherClassTimeWithPenalties_TeacherId",
                table: "TeacherClassTimeWithPenalties",
                column: "TeacherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeacherClassTimeWithPenalties");

            migrationBuilder.AddColumn<string>(
                name: "PreferredTime",
                table: "Teacher",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
