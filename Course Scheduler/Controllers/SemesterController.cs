using Course_Scheduler.Data;
using Course_Scheduler.Models;
using Course_Scheduler.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Course_Scheduler.Controllers
{
    public class SemesterController : Controller
    {
        private readonly Course_SchedulerContext _countext;

        public SemesterController(Course_SchedulerContext countext)
        {
            _countext = countext;
        }

        public async Task<IActionResult> Index()
        {
            var semesters = await _countext.Semester.ToListAsync();
            return View(semesters);
        }

        public async Task<IActionResult> Create()
        {
            ViewData["Courses"] = await _countext.Courses.ToListAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(AddSemesterViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Courses"] = await _countext.Courses.ToListAsync();
                return View(viewModel);
            }
            Semester semester = new()
            {
                Name = viewModel.Name
            };
            await _countext.Semester.AddAsync(semester);
            await _countext.SaveChangesAsync();
            foreach (var courseId in viewModel.CoursesId)
            {
                await _countext.CourseToSemester.AddAsync(new()
                {
                    CourseID = courseId,
                    SemesterID = semester.ID
                });
            }
            await _countext.SaveChangesAsync();
            return View(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var semeter = await _countext.Semester.FirstOrDefaultAsync(x => x.ID == id);
            if (semeter == null)
            {
                return NotFound();
            }
            var viewModel = new AddSemesterViewModel()
            {
                Name = semeter.Name
            };
            var coursesOfThisSemeterId = await _countext.CourseToSemester
                .Where(c => c.SemesterID == id)
                .Select(c => c.CourseID)
                .ToListAsync();
            viewModel.CoursesId = coursesOfThisSemeterId;

            ViewData["Courses"] = await _countext.Courses.ToListAsync();
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(AddSemesterViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Courses"] = await _countext.Courses.ToListAsync();
                return View(viewModel);
            }
            if (viewModel.ID != null)
            {
                return NotFound();
            }
            var semester = await _countext.Semester.FirstOrDefaultAsync(s => s.ID == viewModel.ID);
            if(semester == null)
            {
                return NotFound();
            }

            semester.Name = viewModel.Name;
            _countext.Semester.Update(semester);
            await _countext.SaveChangesAsync();

            var oldCoursesOfThisSemester = await _countext.CourseToSemester
                .Where(c => c.SemesterID == viewModel.ID).ToListAsync();
            _countext.CourseToSemester.RemoveRange(oldCoursesOfThisSemester);
            await _countext.SaveChangesAsync();

            foreach (var courseId in viewModel.CoursesId)
            {
                await _countext.CourseToSemester.AddAsync(new()
                {
                    CourseID = courseId,
                    SemesterID = semester.ID
                });
            }
            await _countext.SaveChangesAsync();
            return View(nameof(Index));
        }


        public async Task<IActionResult> Delete(int id)
        {
            var semeter = await _countext.Semester.FirstOrDefaultAsync(x => x.ID == id);
            if (semeter == null)
            {
                return NotFound();
            }
            _countext.Semester.Remove(semeter);
            return View(nameof(Index));
        }
    }
}
