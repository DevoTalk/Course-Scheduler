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
using Course_Scheduler.Models.Enum;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
            var courses = await _context.Courses.ToListAsync();
            foreach (var course in courses)
            {
                var ctt = await _context.CourseTeacherClassTime.FirstOrDefaultAsync(c => c.Course == course);
                var isFix = false;
                if (ctt != null)
                {
                    isFix = true;
                }
                var teachresIds = await _context.CourseToTeacher.Where(c => c.CourseID == course.ID).Select(p => p.TeacherID).ToListAsync();
                var teachers = new List<Teacher>();
                foreach (var id in teachresIds)
                {
                    teachers.Add(_context.Teacher.First(t => t.ID == id));
                }
                courseAndTeachersViewModel.Add(new()
                {
                    Course = course,
                    Teachers = teachers,
                    IsFix = isFix,
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
            ViewData["Groups"] = _context.Groups.ToList();
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddCourseViewModel courseAndTeachers)
        {
            if (!ModelState.IsValid)
            {
                ViewData["RequiredCourse"] = _context.Courses.ToList();
                ViewData["Teachers"] = _context.Teacher.ToList();
                ViewData["Groups"] = _context.Groups.ToList();
                ModelState.AddModelError("Course.CourseCode", "Course Code mose be UNIQUE");
                return View(courseAndTeachers);
            }
            var isUnic = await _context.Courses.FirstOrDefaultAsync(c => c.CourseCode == courseAndTeachers.Course.CourseCode);
            if (isUnic != null)
            {
                ViewData["RequiredCourse"] = _context.Courses.ToList();
                ViewData["Teachers"] = _context.Teacher.ToList();
                ViewData["Groups"] = _context.Groups.ToList();
                ModelState.AddModelError("Course.CourseCode", "Course Code mose be UNIQUE");
                return View(courseAndTeachers);
            }

            _context.Add(courseAndTeachers.Course);
            await _context.SaveChangesAsync();

            foreach (var groupId in courseAndTeachers.GroupIds)
            {
                _context.CourseToGroups.Add(new()
                {
                    CourseId = courseAndTeachers.Course.ID,
                    GroupId = groupId,
                });
            }
            await _context.SaveChangesAsync();

            foreach (var PrerequisitesId in courseAndTeachers.PrerequisitesId)
            {
                _context.CoursePrerequisites.Add(new()
                {
                    CourseId = courseAndTeachers.Course.ID,
                    PrerequisiteCourseId = PrerequisitesId
                });
            }
            await _context.SaveChangesAsync();

            foreach (var corequisiteId in courseAndTeachers.CorequisitesId)
            {
                _context.CorequisitesCourses.Add(new()
                {
                    CourseId = courseAndTeachers.Course.ID,
                    CorequisiteCourseId = corequisiteId
                });
            }
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

            return RedirectToAction(nameof(Index));
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FirstOrDefaultAsync(c => c.ID == id);
            if (course == null)
            {
                return NotFound();
            }
            var courses = _context.Courses.ToList();
            courses.Remove(course);
            var techersOfCourse = _context.CourseToTeacher.Where(c => c.CourseID == id).Select(c => c.TeacherID).ToList();
            var coursePrerequisitesId = _context.CoursePrerequisites.Where(p => p.CourseId == course.ID).Select(p => p.PrerequisiteCourseId).ToList();
            var CorequisitesCourseId = _context.CorequisitesCourses.Where(p => p.CourseId == course.ID).Select(p => p.CorequisiteCourseId).ToList();
            var GroupsOfThisCourse = _context.CourseToGroups.Where(p => p.CourseId == course.ID).Select(p => p.GroupId).ToList();

            var viewmodel = new AddCourseViewModel()
            {
                Course = course,
                TeachersId = techersOfCourse,
                PrerequisitesId = coursePrerequisitesId,
                CorequisitesId = CorequisitesCourseId,
                GroupIds = GroupsOfThisCourse,
            };
            ViewData["RequiredCourse"] = courses;
            ViewData["Teachers"] = _context.Teacher.ToList();
            ViewData["Groups"] = _context.Groups.ToList();
            return View(viewmodel);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AddCourseViewModel viewModel)
        {
            if (id != viewModel.Course.ID)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                ViewData["Teachers"] = _context.Teacher.ToList();
                ViewData["RequiredCourse"] = _context.Courses.ToList();
                ViewData["Groups"] = _context.Groups.ToList();

                return View(viewModel);
            }
            var isUnic = await _context.Courses.FirstOrDefaultAsync(c => c.CourseCode == viewModel.Course.CourseCode && c.ID != viewModel.Course.ID);
            if (isUnic != null)
            {
                ViewData["RequiredCourse"] = _context.Courses.ToList();
                ViewData["Teachers"] = _context.Teacher.ToList();
                ViewData["Groups"] = _context.Groups.ToList();

                ModelState.AddModelError("Course.CourseCode", "Course Code mose be UNIQUE");
                return View(viewModel);
            }
            var removeOldTecherOfCourse = _context.CourseToTeacher.Where(c => c.CourseID == viewModel.Course.ID);
            _context.CourseToTeacher.RemoveRange(removeOldTecherOfCourse);

            var removeOldPrerequisitesOfCourse = _context.CoursePrerequisites.Where(c => c.CourseId == viewModel.Course.ID);
            _context.CoursePrerequisites.RemoveRange(removeOldPrerequisitesOfCourse);

            var removeOldCorequisitesCourse = _context.CorequisitesCourses.Where(c => c.CourseId == viewModel.Course.ID);
            _context.CorequisitesCourses.RemoveRange(removeOldCorequisitesCourse);

            var removeOldGroups = _context.CourseToGroups.Where(p => p.CourseId == viewModel.Course.ID);
            _context.CourseToGroups.RemoveRange(removeOldGroups);

            _context.Update(viewModel.Course);
            await _context.SaveChangesAsync();

            foreach (var groupId in viewModel.GroupIds)
            {
                _context.CourseToGroups.Add(new()
                {
                    CourseId = viewModel.Course.ID,
                    GroupId = groupId,
                });
            }
            await _context.SaveChangesAsync();

            foreach (var TeacherId in viewModel.TeachersId)
            {
                _context.CourseToTeacher.Add(new()
                {
                    Course = viewModel.Course,
                    TeacherID = TeacherId
                });
            }
            foreach (var PrerequisiteId in viewModel.PrerequisitesId)
            {
                _context.CoursePrerequisites.Add(new()
                {
                    CourseId = viewModel.Course.ID,
                    PrerequisiteCourseId = PrerequisiteId
                });
            }
            foreach (var CorequisiteId in viewModel.CorequisitesId)
            {
                _context.CorequisitesCourses.Add(new()
                {
                    CourseId = viewModel.Course.ID,
                    CorequisiteCourseId = CorequisiteId
                });
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
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
                //var coursesToUpdate = _context.Courses.Where(c => c.PrerequisiteID == id);
                //foreach (var item in coursesToUpdate)
                //{
                //    item.PrerequisiteID = null;
                //}
                var coursePrerequisites = _context.CoursePrerequisites.Where(c => c.CourseId == id);
                foreach (var coursePrerequisite in coursePrerequisites)
                {
                    _context.CoursePrerequisites.Remove(coursePrerequisite);
                }
                var teacherCourse = _context.CourseToTeacher.Where(c => c.CourseID == id);
                foreach (var item in teacherCourse)
                {
                    _context.Remove(item);
                }

                var coursePenalty = _context.CoursePenalty.Where(c => c.CourseID == id || c.CourseWithPenaltyID == id);
                foreach (var item in coursePenalty)
                {
                    _context.Remove(item);
                }
                _context.Courses.Remove(course);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }





        public async Task<IActionResult> FixCourseTime(int id)
        {
            var course = await _context.Courses.FirstAsync(c => c.ID == id);
            var teachersOfThisCourse = await _context.CourseToTeacher
                .Include(tc => tc.Teacher)
                    .Where(tc => tc.CourseID == id)
                        .Select(tc => tc.Teacher).ToListAsync();
            foreach (var teacher in teachersOfThisCourse)
            {
                var freeTimes = await _context.TeacherClassTimeWithPenalties.Where(tp => tp.TeacherId == teacher.ID).ToListAsync();
                teacher.PreferredTimes = freeTimes;
            }
            var semesters = await _context.Semesters.ToListAsync();
            var fixCourseTimeViewModel = new FixCourseTimeViewModel()
            {
                CourseId = id,
                Teachers = teachersOfThisCourse,
                CourseCredits = course.Credits,
            };
            ViewBag["semester"] = await _context.Semesters.ToListAsync();
            return View(fixCourseTimeViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> FixCourseTime(FixCourseTimeViewModel fixCourseTimeViewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag["semester"] = await _context.Semesters.ToListAsync();
                return RedirectToAction(nameof(FixCourseTime), fixCourseTimeViewModel.CourseId);
            }
            var course = await _context.Courses.FirstAsync(c => c.ID == fixCourseTimeViewModel.CourseId);
            var teacher = await _context.Teacher.FirstAsync(t => t.ID == fixCourseTimeViewModel.TeacherId);
            var CTT = new CourseTeacherClassTime()
            {
                Course = course,
                Teacher = teacher,
                SemesterId = fixCourseTimeViewModel.SemesterId,
            };
            await _context.CourseTeacherClassTime.AddAsync(CTT);
            await _context.SaveChangesAsync();

            foreach (var time in fixCourseTimeViewModel.Times)
            {
                if (time.ClassTime != null && time.EvenOdd != null)
                {
                    await _context.EvenOddClassTime.AddAsync(new()
                    {
                        ClassTime = (ClassTimes)time.ClassTime,
                        EvenOdd = time.EvenOdd,
                        CourseTeacherClass = CTT
                    });
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> UnFixCourse(int id)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.ID == id);
            if (course != null)
            {
                var Ctt = await _context.CourseTeacherClassTime.FirstOrDefaultAsync(c => c.Course == course);
                if (Ctt != null)
                {
                    var times = _context.EvenOddClassTime.Where(t => t.CourseTeacherClassTimeId == Ctt.ID).ToList();

                    _context.CourseTeacherClassTime.Remove(Ctt);
                    _context.EvenOddClassTime.RemoveRange(times);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.ID == id);
        }
    }
}
