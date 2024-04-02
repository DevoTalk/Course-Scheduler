using Course_Scheduler.Data;
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
        public IActionResult GeneticAlgorithm(int Count = 100) 
        {
            var courses = _context.Courses.ToList();
            var courseToTeachers = _context.CourseToTeacher.ToList();
            var coursePenaltys = _context.CoursePenalty.ToList();
            var teachers = _context.Teacher.ToList();
            GeneticAlgorithm ga = new(courses, courseToTeachers, coursePenaltys, teachers);
            var a = ga.GeneratePopulation(Count);
            a = a.OrderBy(s => s.TotalPenalty).ToList();
            return View("Schedule",a);
        }

       
    }
}
