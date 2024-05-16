using ClosedXML.Excel;
using Course_Scheduler.Data;
using Course_Scheduler.Models;
using Course_Scheduler.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;

namespace Course_Scheduler.Controllers
{
    public class SchedulerController : Controller
    {
        private readonly Course_SchedulerContext _context;

        public SchedulerController(Course_SchedulerContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["semester"]= await _context.Semesters.ToListAsync();
            return View();
        }
        public async Task<IActionResult> GeneticAlgorithm(int semesterId,int Count = 100, int UnhealtyCount = 1000) 
        {
            var semester = await _context.Semesters.FirstOrDefaultAsync(s => s.ID == semesterId);
            var coursesId = _context.CourseToSemester
                .Where(c => c.SemesterID == semesterId)
                .Select(c => c.CourseID)
                .ToList();
            var courses = new List<Course>();
            foreach (var courseId in coursesId)
            {
                courses.Add(await _context.Courses.FirstAsync(c => c.ID == courseId));
            }
            var courseToTeachers = _context.CourseToTeacher.ToList();
            var coursePenaltys = _context.CoursePenalty.ToList();
            var teachers = _context.Teacher.Include(t => t.PreferredTimes).ToList();
            var fixedCourses = _context.CourseTeacherClassTime
                .Include(ctt => ctt.ClassTimes)
                .Where(fc => fc.SemesterId == semesterId)
                .ToList();
            var coursePrerequisites = _context.CoursePrerequisites.ToList();
            
            var schedules = new List<Schedule>();

            GeneticAlgorithm ga = new GeneticAlgorithm(courses, courseToTeachers, coursePenaltys,
                teachers, fixedCourses, coursePrerequisites);
            
            schedules.AddRange(await ga.CreateSchedules(Count,UnhealtyCount));
            
            schedules = schedules.Distinct();
            schedules = schedules.OrderBy(s => s.Penalty.TotalPenalty).ToList();


           



            // Create a new Excel workbook
            using (var workbook = new XLWorkbook())
            {
                // Add a worksheet to the workbook
                var worksheet = workbook.Worksheets.Add("My Worksheet");

                // Set headers
                worksheet.Cell(1, 1).Value = "ID";
                worksheet.Cell(1, 2).Value = "Penalty";
                worksheet.Cell(1, 3).Value = "Course name";
                worksheet.Cell(1, 4).Value = "Teacher name";
                worksheet.Cell(1, 5).Value = "Times";

                // Populate data
                int row = 2;
                int id = 0;
                foreach (var schedule in schedules)
                {
                    worksheet.Row(row).Style.Fill.BackgroundColor = XLColor.LightGray;
                    worksheet.Cell(row, 1).Value = id;
                    id++;
                    worksheet.Cell(row, 2).Value = schedule.Penalty.TotalPenalty;
                    row++;
                    foreach (var CTT in schedule.CourseTeacherClassTimes)
                    {
                        worksheet.Cell(row, 3).Value = CTT.Course.Name;
                        worksheet.Cell(row, 4).Value = CTT.Teacher.Name;
                        string times = "";
                        foreach(var time in CTT.ClassTimes)
                        {
                           times += time.ClassTime.ToString()+ "  " + time.EvenOdd + "  ";
                        }
                        worksheet.Cell(row, 5).Value = times;
                        row++;
                    }
                    row++;
                }

                // Save the workbook
                workbook.SaveAs("output.xlsx");
            }


            schedules = schedules.Take(10).ToList();
            return View("Schedule", schedules);
        }

       
    }
}
