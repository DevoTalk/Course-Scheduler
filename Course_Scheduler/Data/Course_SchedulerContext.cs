using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Course_Scheduler.Models;
using Course_Scheduler.Models.Enum;

namespace Course_Scheduler.Data
{
    public class Course_SchedulerContext : DbContext
    {
        public Course_SchedulerContext(DbContextOptions<Course_SchedulerContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CoursePrerequisites>()
                .HasOne(p => p.Course)
                .WithMany(c => c.Prerequisites)
                .HasForeignKey(p => p.CourseId);
            modelBuilder.Entity<Course>()
                .HasIndex(c=>c.CourseCode)
                .IsUnique();
            base.OnModelCreating(modelBuilder);

        }

        public DbSet<Course_Scheduler.Models.Teacher> Teacher { get; set; } = default!;
        public DbSet<Course_Scheduler.Models.Course> Courses { get; set; } = default!;
        public DbSet<Course_Scheduler.Models.CourseToTeacher> CourseToTeacher { get; set; } = default!;
        public DbSet<Course_Scheduler.Models.CoursePenalty> CoursePenalty { get; set; } = default!;
        public DbSet<Course_Scheduler.Models.CoursePrerequisites> CoursePrerequisites { get; set; } = default!;
        public DbSet<Course_Scheduler.Models.CorequisiteCourse> CorequisitesCourses { get; set; } = default!;
        public DbSet<Course_Scheduler.Models.TeacherClassTimeWithPenalties> TeacherClassTimeWithPenalties { get; set; } = default!;
        public DbSet<Course_Scheduler.Models.CourseTeacherClassTime> CourseTeacherClassTime { get; set; } = default!;
        public DbSet<Course_Scheduler.Models.EvenOddClassTime> EvenOddClassTime { get; set; } = default!;
        public DbSet<Course_Scheduler.Models.Semester> Semesters { get; set; } = default!;
        public DbSet<Course_Scheduler.Models.CourseToSemester> CourseToSemester { get; set; } = default!;
        public DbSet<Course_Scheduler.Models.Group> Groups { get; set; } = default!;
        public DbSet<Course_Scheduler.Models.CourseToGroups> CourseToGroups { get; set; } = default!;

    }
}
