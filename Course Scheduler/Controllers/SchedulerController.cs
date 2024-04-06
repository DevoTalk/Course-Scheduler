using Course_Scheduler.Data;
using Course_Scheduler.Models;
using Course_Scheduler.Services;
using Microsoft.AspNetCore.Mvc;

namespace Course_Scheduler.Controllers
{
    public class SchedulerController : Controller
    {
        private readonly Course_SchedulerContext _context;

        public SchedulerController(Course_SchedulerContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GeneticAlgorithm(int Count = 100) 
        {
            var courses = _context.Courses.ToList();
            var courseToTeachers = _context.CourseToTeacher.ToList();
            var coursePenaltys = _context.CoursePenalty.ToList();
            var teachers = _context.Teacher.ToList();
            GeneticAlgorithm ga = new(courses, courseToTeachers, coursePenaltys, teachers);
            var tasks = new List<Task<List<Schedule>>>();
            for (int i = 0; i < Count / 100; i++)
            {
                tasks.Add(ga.GeneratePopulation(100));
            }
            await Task.WhenAll(tasks);
            var schedules = new List<Schedule>();
            foreach (var task in tasks)
            {
                schedules.AddRange(task.Result);
            }
            schedules = schedules.Distinct();
            schedules = schedules.OrderBy(s => s.TotalPenalty).ToList();
            return View("Schedule", schedules);
        }

       
    }
}
