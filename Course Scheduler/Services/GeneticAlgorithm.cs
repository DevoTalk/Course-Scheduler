using Course_Scheduler.Models;
using Course_Scheduler.Models.Enum;

namespace Course_Scheduler.Services;

public class GeneticAlgorithm
{
    public GeneticAlgorithm(List<Course> courses, List<CourseToTeacher> courseToTeacher, List<CoursePenalty> coursePenalties, List<Teacher> teachers)
    {
        Courses = courses;
        CourseToTeacher = courseToTeacher;
        CoursePenalties = coursePenalties;
        Teachers = teachers;
        
    }

    public List<Course> Courses { get; set; }
    public List<CourseToTeacher> CourseToTeacher { get; set; }
    public List<CoursePenalty> CoursePenalties { get; set; }
    public List<Teacher> Teachers { get; set; }

    public List<Schedule> GenratePopulation(int Count = 100)
    {
        var schedules = new List<Schedule>();
        for (int i = 0; i <= Count; i++)
        {
            var schedule = new Schedule();
            var courentCourseList = new List<Course>();
            courentCourseList.AddRange(Courses);
            var courentTeacherList = new List<Teacher>();
            foreach (var techer in Teachers)
            {
                var t = new Teacher()
                {
                    ID = techer.ID,
                    Name = techer.Name,
                };
                t.PreferredTime.AddRange(techer.PreferredTime);
                courentTeacherList.Add(t);
            }
            for (int j = 1; j <= Courses.Count(); j++)
            {
                var CTT = new CourseTeacherClassTime();
                var rnd = new Random();
                while (true)
                {
                    var index = rnd.Next(Courses.Count());
                    if (courentCourseList.Any(c => c == Courses[index]))
                    {
                        courentCourseList.Remove(Courses[index]);
                        CTT.Course = Courses[index];
                        break;
                    }
                }
                var teacherOfThisCourse = CourseToTeacher.Where(ct => ct.CourseID == CTT.Course.ID).ToList();
                while (true)
                {
                    if (teacherOfThisCourse.Count() != 0)
                    {
                        var teacherId = teacherOfThisCourse[rnd.Next(teacherOfThisCourse.Count())].TeacherID;
                        var teacher = courentTeacherList.First(t => t.ID == teacherId);
                        if (teacher.PreferredTime.Count != 0)
                        {
                            var time = teacher.PreferredTime[rnd.Next(teacher.PreferredTime.Count())];
                            courentTeacherList.First(t => t == teacher).PreferredTime.Remove(time);
                            CTT.Teacher = teacher;
                            CTT.ClassTime = time;
                            break;
                        }
                        teacherOfThisCourse.Remove(teacherOfThisCourse.First(tc => tc.TeacherID == teacherId));
                    }
                }
                schedule.CourseTeacherClassTime.Add(CTT);
            }
            schedules.Add(schedule);
        }
        return schedules;
    }
}
