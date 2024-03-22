using Course_Scheduler.Data;
using Course_Scheduler.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Course_Scheduler.Controllers
{
    public class PenaltyController : Controller
    {
        private readonly Course_SchedulerContext _context;

        public PenaltyController(Course_SchedulerContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var courses = await _context.Courses.ToListAsync();
            return View(courses);
        }

        public async Task<IActionResult> Edit(int id) 
        {
            var viewModel = new List<CoursePenaltyViewModel>();
            ViewData["Course"] = await _context.Courses.FirstAsync(c => c.ID == id);
            var penaltyCourses = await _context.CoursePenalty.Where(cp => cp.CourseID == id).ToListAsync();
            foreach (var penaltyCourse in penaltyCourses)
            {
                var PCourse = await _context.Courses.FirstOrDefaultAsync(c => c.ID == penaltyCourse.CourseWithPenaltyID);
                viewModel.Add(new CoursePenaltyViewModel
                {
                    CourseWithPenalty = PCourse,
                    PenaltyCount = penaltyCourse.PenaltyCount
                });
            }
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, List<CoursePenaltyViewModel> coursePenaltyViewModels)
        {
            //var viewModel = new List<CoursePenaltyViewModel>();
            var coursePenalty = await _context.CoursePenalty.Where(cp => cp.CourseID == id).ToListAsync();
            foreach (var cp in coursePenalty)
            {
                foreach(var editedCp in coursePenaltyViewModels)
                {
                    if (editedCp.CourseWithPenalty.ID == cp.CourseWithPenaltyID)
                    {
                        if(editedCp.PenaltyCount != cp.PenaltyCount)
                        {
                            cp.PenaltyCount = editedCp.PenaltyCount;
                            _context.Update(cp);
                            await _context.SaveChangesAsync();
                        }
                    }
                }
                //var CoursPenalty = _context.Courses.First(c => c.ID == cp.CourseWithPenaltyID);
                //viewModel.Add(new()
                //{
                //    CourseWithPenalty = CoursPenalty,
                //    PenaltyCount = cp.PenaltyCount,
                //});
            }
            // ViewData["Course"] = await _context.Courses.FirstAsync(c => c.ID == id);
            return RedirectToAction(nameof(Index));
        }
    }
}
