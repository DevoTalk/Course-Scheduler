﻿using ClosedXML.Excel;
using Course_Scheduler.Data;
using Course_Scheduler.Models;
using Course_Scheduler.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> GeneticAlgorithm(int Count = 100) 
        {
            var courses = _context.Courses.ToList();
            var courseToTeachers = _context.CourseToTeacher.ToList();
            var coursePenaltys = _context.CoursePenalty.ToList();
            var teachers = _context.Teacher.Include(t => t.PreferredTimes).ToList();
            var fixedCourses = _context.CourseTeacherClassTime.Include(ctt => ctt.ClassTimes).ToList();
            GeneticAlgorithm ga = new(courses, courseToTeachers, coursePenaltys, teachers, fixedCourses);
            //var tasks = new List<Task<List<Schedule>>>();
            //for (int i = 0; i < Count / 100; i++)
            //{
            //    tasks.Add(ga.GeneratePopulation(100));
            //}
            //await Task.WhenAll(tasks);
            var schedules = new List<Schedule>();
            schedules.AddRange(await ga.GeneratePopulation(100));
            //foreach (var task in tasks)
            //{
            //    schedules.AddRange(task.Result);
            //}
            schedules = schedules.Distinct();
            schedules = schedules.OrderBy(s => s.TotalPenalty).ToList();






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
                    worksheet.Cell(row, 2).Value = schedule.TotalPenalty;
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
