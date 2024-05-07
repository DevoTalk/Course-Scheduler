using Course_Scheduler.Data;
using Course_Scheduler.Models;
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

            var course = await _context.Courses.FirstAsync(c => c.ID == id);
            ViewData["Course"] = course;
            var penaltyCourses = await _context.CoursePenalty.Where(cp => cp.CourseID == id || cp.CourseWithPenaltyID == id).ToListAsync();

            foreach (var penaltyCourse in penaltyCourses)
            {
                Course PCourse;
                if (penaltyCourse.CourseID == id)
                {
                    PCourse = await _context.Courses.FirstAsync(c => c.ID == penaltyCourse.CourseWithPenaltyID);
                }
                else
                {
                    PCourse = await _context.Courses.FirstAsync(c => c.ID == penaltyCourse.CourseID);
                }
                viewModel.Add(new CoursePenaltyViewModel
                {
                    CourseWithPenalty = PCourse,
                    PenaltyCount = penaltyCourse.PenaltyCount
                });

            }
            return View(viewModel);
        }
        [HttpPost]
        //changeeeee
        public async Task<IActionResult> Edit(int id, List<CoursePenaltyViewModel> coursePenaltyViewModels)
        {
            foreach (var item in coursePenaltyViewModels)
            {
                var cp = await _context.CoursePenalty.FirstAsync(cp =>
                    (cp.CourseID == id && cp.CourseWithPenaltyID == item.CourseWithPenalty.ID) ||
                    (cp.CourseID == item.CourseWithPenalty.ID && cp.CourseWithPenaltyID == id));
                cp.PenaltyCount = item.PenaltyCount;
                _context.Update(cp);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> GeneratePenalty()
        {
            var penaltyToRemove = await _context.CoursePenalty.ToListAsync();
            _context.CoursePenalty.RemoveRange(penaltyToRemove);
            await _context.SaveChangesAsync();

            var coursePenaltys = new List<CoursePenalty>();
            var courses = await _context.Courses.Include(c => c.Prerequisites).ToListAsync();
            foreach (var course in courses)
            {
                foreach (var courseToAdd in courses)
                {
                    if (!(coursePenaltys.Any(cp => cp.CourseID == course.ID && cp.CourseWithPenaltyID == courseToAdd.ID) ||
                        coursePenaltys.Any(cp => cp.CourseID == courseToAdd.ID && cp.CourseWithPenaltyID == course.ID)))
                    {
                        if (courseToAdd.ID != course.ID)
                        {
                            var penalty = 0;
                            var coursePrerequisites = _context.CoursePrerequisites.Where(c => c.ID == course.ID);
                            var courseToAddPrerequisites = _context.CoursePrerequisites.Where(c => c.ID == courseToAdd.ID);
                            if (coursePrerequisites.Select(c => c.PrerequisiteCourseId).Order() == courseToAddPrerequisites.Select(c => c.PrerequisiteCourseId).Order())
                            {
                                penalty = 1;
                            }
                            coursePenaltys.Add(new()
                            {
                                Course = course,
                                CourseID = course.ID,
                                CourseWithPenaltyID = courseToAdd.ID,
                                PenaltyCount = penalty
                            });
                        }
                    }
                }
            }
            await _context.CoursePenalty.AddRangeAsync(coursePenaltys);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
