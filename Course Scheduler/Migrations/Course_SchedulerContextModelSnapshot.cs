﻿// <auto-generated />
using System;
using Course_Scheduler.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Course_Scheduler.Migrations
{
    [DbContext(typeof(Course_SchedulerContext))]
    partial class Course_SchedulerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.3");

            modelBuilder.Entity("Course_Scheduler.Models.Course", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CountOfClass")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Credits")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("PrerequisiteID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("PrerequisiteID");

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

            modelBuilder.Entity("Course_Scheduler.Models.CourseTeacherClassTime", b =>
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

                    b.ToTable("CourseTeacherClassTime");
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

            modelBuilder.Entity("Course_Scheduler.Models.Course", b =>
                {
                    b.HasOne("Course_Scheduler.Models.Course", "Prerequisite")
                        .WithMany()
                        .HasForeignKey("PrerequisiteID");

                    b.Navigation("Prerequisite");
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
