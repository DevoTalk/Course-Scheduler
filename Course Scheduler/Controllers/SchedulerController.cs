using Course_Scheduler.Data;
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

        public IActionResult TopologicalSort()
        {
            var courses = _context.Courses.ToList();
            
            return View();
        }
    }
}
