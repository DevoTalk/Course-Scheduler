using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Course_Scheduler.Data;
using Course_Scheduler.Models;
using Course_Scheduler.Models.ViewModels;
using NuGet.DependencyResolver;

namespace Course_Scheduler.Controllers
{
    public class CoursesController : Controller
    {
        private readonly Course_SchedulerContext _context;

        public CoursesController(Course_SchedulerContext context)
        {
            _context = context;
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
            var courseAndTeachersViewModel = new List<CourseAndTeachersViewModel>();
            var courses= await _context.Courses.Include(c => c.Prerequisite).ToListAsync();
            foreach(var course in courses)
            {
                var teachresIds = await _context.CourseToTeacher.Where(c => c.CourseID == course.ID).Select(p => p.TeacherID).ToListAsync();
                var teachers = new List<Teacher>();
                foreach (var id in teachresIds)
                {
                     teachers.Add(_context.Teacher.First(t => t.ID == id));
                }
                courseAndTeachersViewModel.Add(new()
                {
                    Course = course,
                    Teachers = teachers
                });
            }

            return View(courseAndTeachersViewModel);
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.Prerequisite)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (course == null)
            {
                return NotFound();
            }
            var teachresIds = await _context.CourseToTeacher.Where(c => c.CourseID == course.ID).Select(p => p.TeacherID).ToListAsync();
            var teachers = new List<Teacher>();
            foreach (var teachreId in teachresIds)
            {
                teachers.Add(_context.Teacher.First(t => t.ID == teachreId));
            }
            CourseAndTeachersViewModel viewModel = new CourseAndTeachersViewModel()
            {
                Course = course,
                Teachers = teachers
            };
            return View(viewModel);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            ViewData["RequiredCourse"] = _context.Courses.ToList();
            ViewData["Teachers"] = _context.Teacher.ToList();
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddCourseViewModel courseAndTeachers)
        {
            if (ModelState.IsValid)
            {
                _context.Add(courseAndTeachers.Course);
                await _context.SaveChangesAsync();
                foreach (var teacherid in courseAndTeachers.TeachersId)
                {
                    _context.CourseToTeacher.Add(new()
                    {
                        Course = courseAndTeachers.Course,
                        TeacherID = teacherid
                    });
                }
                await _context.SaveChangesAsync();
                var courses = await _context.Courses.ToListAsync();
                foreach (var course in courses) {
                    _context.CoursePenalty.Add(new()
                    {
                        Course = courseAndTeachers.Course,
                        CourseWithPenaltyID = course.ID,
                        PenaltyCount = 0
                    });
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RequiredCourse"] = _context.Courses.ToList();
            ViewData["RequiredCourseID"] = new SelectList(_context.Courses, "ID", "ID", courseAndTeachers.Course.PrerequisiteID);
            ViewData["Teachers"] = _context.Teacher.ToList();

            return View(courseAndTeachers);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            var courses = _context.Courses.ToList();
            courses.Remove(course);
            ViewData["RequiredCourse"] = courses;
            ViewData["Teachers"] = _context.Teacher.ToList();
            var techersOfCourse = _context.CourseToTeacher.Where(c => c.CourseID == id).Select(c => c.TeacherID).ToList();
            var viewmodel = new AddCourseViewModel()
            {
                Course = course,
                TeachersId = techersOfCourse
            };
            return View(viewmodel);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,AddCourseViewModel viewModel)
        {
            if (id != viewModel.Course.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var removeOldTecherOfCourse = _context.CourseToTeacher.Where(c => c.CourseID == viewModel.Course.ID);
                    foreach (var teacher in removeOldTecherOfCourse)
                    {
                        _context.CourseToTeacher.Remove(teacher);
                    }
                    _context.Update(viewModel.Course);
                    foreach(var TeacherId in viewModel.TeachersId)
                    {
                        _context.CourseToTeacher.Add(new()
                        {
                            Course = viewModel.Course,
                            TeacherID = TeacherId
                        });
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(viewModel.Course.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RequiredCourseID"] = new SelectList(_context.Courses, "ID", "ID", viewModel.Course.PrerequisiteID);
            return View(viewModel);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.Prerequisite)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                var coursesToUpdate = _context.Courses.Where(c => c.PrerequisiteID == id);
                foreach (var item in coursesToUpdate)
                {
                    item.PrerequisiteID = null;
                }
                var teacherCourse = _context.CourseToTeacher.Where(c => c.CourseID == id);
                foreach (var item in teacherCourse)
                {
                    _context.Remove(item);
                }

                _context.Courses.Remove(course);
                
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.ID == id);
        }
    }
}
