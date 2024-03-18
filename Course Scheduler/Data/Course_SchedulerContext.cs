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
        public Course_SchedulerContext (DbContextOptions<Course_SchedulerContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Teacher>().Property(p => p.PreferredTime)
                .HasConversion(
                p => string.Join(",", p),
                p => p.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(l => Enum.Parse<ClassTime>(l)).ToList());
            
            base.OnModelCreating(modelBuilder);
            
        }
        
        public DbSet<Course_Scheduler.Models.Teacher> Teacher { get; set; } = default!;
        public DbSet<Course_Scheduler.Models.Course> Courses { get; set; } = default!;
        public DbSet<Course_Scheduler.Models.CourseToTeacher> CourseToTeacher { get; set; } = default!;
    }
}
