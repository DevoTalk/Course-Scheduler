﻿// <auto-generated />
using Course_Scheduler.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Course_Scheduler.Migrations
{
    [DbContext(typeof(Course_SchedulerContext))]
    [Migration("20240517152342_tftf")]
    partial class tftf
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.5");

            modelBuilder.Entity("CourseGroup", b =>
                {
                    b.Property<int>("CoursesID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("GroupsID")
                        .HasColumnType("INTEGER");

                    b.HasKey("CoursesID", "GroupsID");

                    b.HasIndex("GroupsID");

                    b.ToTable("CourseGroup");
                });

            modelBuilder.Entity("Course_Scheduler.Models.CorequisiteCourse", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CorequisiteCourseId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CourseId")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("CourseId");

                    b.ToTable("CorequisitesCourses");
                });

            modelBuilder.Entity("Course_Scheduler.Models.Course", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CountOfClass")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CourseCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Credits")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("CourseCode")
                        .IsUnique();

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("Course_Scheduler.Models.CoursePenalty", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CourseID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CourseWithPenaltyID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PenaltyCount")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("CourseID");

                    b.ToTable("CoursePenalty");
                });

            modelBuilder.Entity("Course_Scheduler.Models.CoursePrerequisites", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CourseId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PrerequisiteCourseId")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("CourseId");

                    b.ToTable("CoursePrerequisites");
                });

            modelBuilder.Entity("Course_Scheduler.Models.CourseTeacherClassTime", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CourseID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SemesterId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TeacherID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("CourseID");

                    b.HasIndex("TeacherID");

                    b.ToTable("CourseTeacherClassTime");
                });

            modelBuilder.Entity("Course_Scheduler.Models.CourseToGroups", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CourseId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("GroupId")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("CourseId");

                    b.HasIndex("GroupId");

                    b.ToTable("CourseToGroups");
                });

            modelBuilder.Entity("Course_Scheduler.Models.CourseToSemester", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CourseID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SemesterID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("CourseID");

                    b.HasIndex("SemesterID");

                    b.ToTable("CourseToSemester");
                });

            modelBuilder.Entity("Course_Scheduler.Models.CourseToTeacher", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CourseID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TeacherID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("CourseID");

                    b.HasIndex("TeacherID");

                    b.ToTable("CourseToTeacher");
                });

            modelBuilder.Entity("Course_Scheduler.Models.EvenOddClassTime", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ClassTime")
                        .HasColumnType("INTEGER");

                    b.Property<int>("CourseTeacherClassTimeId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("EvenOdd")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("CourseTeacherClassTimeId");

                    b.ToTable("EvenOddClassTime");
                });

            modelBuilder.Entity("Course_Scheduler.Models.Group", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("Course_Scheduler.Models.Semester", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Semesters");
                });

            modelBuilder.Entity("Course_Scheduler.Models.Teacher", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("MaximumDayCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("PenaltyForEmptyTime")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TeacherCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Teacher");
                });

            modelBuilder.Entity("Course_Scheduler.Models.TeacherClassTimeWithPenalties", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("EvenOdd")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Penalty")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PreferredTime")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TeacherId")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("TeacherId");

                    b.ToTable("TeacherClassTimeWithPenalties");
                });

            modelBuilder.Entity("CourseGroup", b =>
                {
                    b.HasOne("Course_Scheduler.Models.Course", null)
                        .WithMany()
                        .HasForeignKey("CoursesID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Course_Scheduler.Models.Group", null)
                        .WithMany()
                        .HasForeignKey("GroupsID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Course_Scheduler.Models.CorequisiteCourse", b =>
                {
                    b.HasOne("Course_Scheduler.Models.Course", "Course")
                        .WithMany("CorequisiteCourses")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");
                });

            modelBuilder.Entity("Course_Scheduler.Models.CoursePenalty", b =>
                {
                    b.HasOne("Course_Scheduler.Models.Course", "Course")
                        .WithMany()
                        .HasForeignKey("CourseID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");
                });

            modelBuilder.Entity("Course_Scheduler.Models.CoursePrerequisites", b =>
                {
                    b.HasOne("Course_Scheduler.Models.Course", "Course")
                        .WithMany("Prerequisites")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");
                });

            modelBuilder.Entity("Course_Scheduler.Models.CourseTeacherClassTime", b =>
                {
                    b.HasOne("Course_Scheduler.Models.Course", "Course")
                        .WithMany()
                        .HasForeignKey("CourseID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Course_Scheduler.Models.Teacher", "Teacher")
                        .WithMany()
                        .HasForeignKey("TeacherID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("Course_Scheduler.Models.CourseToGroups", b =>
                {
                    b.HasOne("Course_Scheduler.Models.Course", "Course")
                        .WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Course_Scheduler.Models.Group", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Group");
                });

            modelBuilder.Entity("Course_Scheduler.Models.CourseToSemester", b =>
                {
                    b.HasOne("Course_Scheduler.Models.Course", "Course")
                        .WithMany()
                        .HasForeignKey("CourseID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Course_Scheduler.Models.Semester", "semester")
                        .WithMany()
                        .HasForeignKey("SemesterID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("semester");
                });

            modelBuilder.Entity("Course_Scheduler.Models.CourseToTeacher", b =>
                {
                    b.HasOne("Course_Scheduler.Models.Course", "Course")
                        .WithMany()
                        .HasForeignKey("CourseID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Course_Scheduler.Models.Teacher", "Teacher")
                        .WithMany()
                        .HasForeignKey("TeacherID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("Course_Scheduler.Models.EvenOddClassTime", b =>
                {
                    b.HasOne("Course_Scheduler.Models.CourseTeacherClassTime", "CourseTeacherClass")
                        .WithMany("ClassTimes")
                        .HasForeignKey("CourseTeacherClassTimeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CourseTeacherClass");
                });

            modelBuilder.Entity("Course_Scheduler.Models.TeacherClassTimeWithPenalties", b =>
                {
                    b.HasOne("Course_Scheduler.Models.Teacher", "Teacher")
                        .WithMany("PreferredTimes")
                        .HasForeignKey("TeacherId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("Course_Scheduler.Models.Course", b =>
                {
                    b.Navigation("CorequisiteCourses");

                    b.Navigation("Prerequisites");
                });

            modelBuilder.Entity("Course_Scheduler.Models.CourseTeacherClassTime", b =>
                {
                    b.Navigation("ClassTimes");
                });

            modelBuilder.Entity("Course_Scheduler.Models.Teacher", b =>
                {
                    b.Navigation("PreferredTimes");
                });
#pragma warning restore 612, 618
        }
    }
}
