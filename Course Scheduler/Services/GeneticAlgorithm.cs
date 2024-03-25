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

    private class CalculatedCoursePenalty
    {
        public Course Course1 { get; set; }
        public Course Course2 { get; set; }
    }
    public int CalculatePenalty(Schedule schedule)
    {
        var penalty = 0;
        var calculatedCoursePenalty = new List<CalculatedCoursePenalty>();
        foreach (var CTT1 in schedule.CourseTeacherClassTime)
        {
            foreach (var CTT2 in schedule.CourseTeacherClassTime)
            {
                if (CTT1 != CTT2)
                {
                    foreach (var ct in CTT1.ClassTime)
                    {
                        foreach (var ct2 in CTT2.ClassTime)
                        {
                            if (ct.ClassTime == ct2.ClassTime)
                            {
                                if (ct.EvenOdd == ct2.EvenOdd)
                                {
                                    if (CTT1.Course.PrerequisiteID == CTT2.Course.PrerequisiteID)
                                    {
                                        if (!calculatedCoursePenalty
                                            .Any(c => c.Course1 == CTT1.Course && c.Course2 == CTT2.Course ||
                                                c.Course1 == CTT2.Course && c.Course2 == CTT1.Course))
                                        {

                                            penalty += CoursePenalties.First(
                                                c => c.CourseID == CTT1.Course.ID && c.CourseWithPenaltyID == CTT2.Course.ID ||
                                                c.CourseID == CTT2.Course.ID && c.CourseWithPenaltyID == CTT1.Course.ID).PenaltyCount;

                                            calculatedCoursePenalty.Add(new()
                                            {
                                                Course1 = CTT1.Course,
                                                Course2 = CTT2.Course,
                                            });
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        return penalty;
    }
    public List<int> CalculatePenalty(List<Schedule> schedules)
    {
        var penaltys = new List<int>();

        foreach (var schedule in schedules)
        {
            var penalty = 0;
            var calculatedCoursePenalty = new List<CalculatedCoursePenalty>();
            foreach (var item in schedule.CourseTeacherClassTime)
            {
                foreach (var item2 in schedule.CourseTeacherClassTime)
                {
                    if (item != item2)
                    {
                        foreach (var ct in item.ClassTime)
                        {
                            foreach (var ct2 in item2.ClassTime)
                            {
                                if (ct == ct2)
                                {
                                    if (item.Course.PrerequisiteID == item2.Course.PrerequisiteID)
                                    {
                                        if (!calculatedCoursePenalty
                                            .Any(c => c.Course1 == item.Course && c.Course2 == item2.Course ||
                                                c.Course1 == item2.Course && c.Course2 == item.Course))
                                        {
                                            penalty += CoursePenalties.First(
                                                c => c.CourseID == CTT1.Course.ID && c.CourseWithPenaltyID == CTT2.Course.ID ||
                                                c.CourseID == CTT2.Course.ID && c.CourseWithPenaltyID == CTT1.Course.ID).PenaltyCount;

                                            calculatedCoursePenalty.Add(new()
                                            {
                                                Course1 = item.Course,
                                                Course2 = item2.Course,
                                            });
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            penaltys.Add(penalty);
        }
        return penaltys;
    }
    public List<Schedule> GeneratePopulation(int Count = 100)
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
                        if (teacher.PreferredTime.Count*2 >= CTT.Course.Credits)
                        {
                            var courseCredits = CTT.Course.Credits;
                            while (courseCredits > 0)
                            {
                                var time = teacher.PreferredTime[rnd.Next(teacher.PreferredTime.Count())];
                                courentTeacherList.First(t => t == teacher).PreferredTime.Remove(time);
                                CTT.Teacher = teacher;
                                if (courseCredits == 1)
                                {

                                    if (rnd.NextDouble() > 0.5)
                                    {
                                        CTT.ClassTime.Add(new()
                                        {
                                            ClassTime = time,
                                            EvenOdd = EvenOdd.odd
                                        });
                                    }
                                    else
                                    {
                                        CTT.ClassTime.Add(new()
                                        {
                                            ClassTime = time,
                                            EvenOdd = EvenOdd.even
                                        });
                                    }
                                }
                                else
                                {
                                    CTT.ClassTime.Add(new()
                                    {
                                        ClassTime = time,
                                        EvenOdd = EvenOdd.everyWeek
                                    });
                                }
                                courseCredits -= 2;

                            }
                            break;
                        }
                        teacherOfThisCourse.Remove(teacherOfThisCourse.First(tc => tc.TeacherID == teacherId));
                    }
                }
                schedule.CourseTeacherClassTime.Add(CTT);
            }
            var penalty = CalculatePenalty(schedule);
            schedule.TotalPenalty = penalty;
            schedules.Add(schedule);
        }
        return schedules;
    }
}
