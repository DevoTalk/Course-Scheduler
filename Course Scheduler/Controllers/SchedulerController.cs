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
            var courses = _context.Courses.ToList();
            var courseToTeachers = _context.CourseToTeacher.ToList();
            var coursePenaltys = _context.CoursePenalty.ToList();
            var teachers = _context.Teacher.ToList();
            GeneticAlgorithm ga = new(courses,courseToTeachers,coursePenaltys,teachers);
            var a =ga.GeneratePopulation(5);
            return View();
        }

        public IActionResult TopologicalSort()
        {
            var courses = _context.Courses.ToList();
            
            return View();
        }
    }
}
