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
using Course_Scheduler.Models.Enum;

namespace Course_Scheduler.Controllers
{
    public class TeachersController : Controller
    {
        private readonly Course_SchedulerContext _context;

        public TeachersController(Course_SchedulerContext context)
        {
            _context = context;
        }

        // GET: Teachers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Teacher.ToListAsync());
        }

        // GET: Teachers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teacher
                .Include(t=>t.PreferredTimes).FirstOrDefaultAsync(m => m.ID == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // GET: Teachers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teachers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddTeacherViewModel teacherViewModel)
        {
            
            if (ModelState.IsValid)
            {
                var teacher = new Teacher()
                {
                    Name = teacherViewModel.Name,
                };
                _context.Add(teacher);
                await _context.SaveChangesAsync();

                foreach (var pt in teacherViewModel.PreferredTime)
                {
                    var teacherPreferredTime = new TeacherClassTimeWithPenalties()
                    {
                        Teacher = teacher,
                        PreferredTime = pt.PreferredTime,
                        Penalty = pt.Penalty
                    };
                    _context.Add(teacherPreferredTime);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(teacherViewModel);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var teacher = await _context.Teacher.Include(t => t.PreferredTimes).FirstOrDefaultAsync(t => t.ID == id);
            if(teacher == null)
            {
                return NotFound();
            }
            var preferredTimeViewModel = new List<TeacherClassTimeWithPenaltiesViewModel>();
            foreach (var item in teacher.PreferredTimes)
            {
                preferredTimeViewModel.Add(new()
                {
                    PreferredTime = item.PreferredTime,
                    Penalty = item.Penalty
                });
            }
            var updateTeacherViewModel = new UpdateTeacherViewModel()
            {
                ID = id,
                Name = teacher.Name,
                PreferredTime = preferredTimeViewModel
            };


            return View(updateTeacherViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id,UpdateTeacherViewModel updateTeacherViewModel)
        {
            var teacher = await _context.Teacher.FirstOrDefaultAsync(t => t.ID == id);
            if (teacher == null)
            {
                return NotFound();
            }
            if(ModelState.IsValid)
            {
                teacher.Name = updateTeacherViewModel.Name;
                _context.Update(teacher);
                var oldTimes = _context.TeacherClassTimeWithPenalties.Where(t => t.TeacherId == id).ToList();
                _context.TeacherClassTimeWithPenalties.RemoveRange(oldTimes);
                foreach (var item in updateTeacherViewModel.PreferredTime)
                {
                    _context.TeacherClassTimeWithPenalties.Add(new()
                    {
                        PreferredTime = item.PreferredTime,
                        Penalty = item.Penalty,
                        TeacherId = id,
                        Teacher = teacher
                    });
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(updateTeacherViewModel);

        }








        // GET: Teachers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teacher
                .FirstOrDefaultAsync(m => m.ID == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacher = await _context.Teacher.FindAsync(id);
            if (teacher != null)
            {
                _context.Teacher.Remove(teacher);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeacherExists(int id)
        {
            return _context.Teacher.Any(e => e.ID == id);
        }
    }
}
