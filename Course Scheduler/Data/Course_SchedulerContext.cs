using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Course_Scheduler.Models;

namespace Course_Scheduler.Data
{
    public class Course_SchedulerContext : DbContext
    {
        public Course_SchedulerContext (DbContextOptions<Course_SchedulerContext> options)
            : base(options)
        {
        }

        public DbSet<Course_Scheduler.Models.Teacher> Teacher { get; set; } = default!;
    }
}
