using Course_Scheduler.Models;
using Course_Scheduler.Models.Enum;
using System.Collections.Generic;
using System.Text;

namespace Course_Scheduler.Services;


public class CoustomTeacherWithEvenOddClassTime : Base
{
    public CoustomTeacherWithEvenOddClassTime()
    {
        PreferredTime = new List<CoustomTime>();
    }
    public string Name { get; set; }
    public List<CoustomTime> PreferredTime { get; set; }
}
public class CoustomTime
{
    public CoustomTime()
    {
        EvenOdd = new();
    }
    public ClassTimes ClassTime { get; set; }
    public List<CoustomEvenOdd>? EvenOdd { get; set; }
}
public enum CoustomEvenOdd
{
    even,
    odd,
    everyWeek,
}
public class GeneticAlgorithm
{
    #region ctor

    public List<Course> Courses { get; set; }
    public List<CourseToTeacher> CourseToTeacher { get; set; }
    public List<CoursePenalty> CoursePenalties { get; set; }
    public List<Teacher> Teachers { get; set; }
    public GeneticAlgorithm(List<Course> courses, List<CourseToTeacher> courseToTeacher, List<CoursePenalty> coursePenalties, List<Teacher> teachers)
    {
        Courses = courses;
        CourseToTeacher = courseToTeacher;
        CoursePenalties = coursePenalties;
        Teachers = teachers;

    }
    #endregion
    #region model and servies
    private class CalculatedCoursePenalty
    {
        public Course Course1 { get; set; }
        public Course Course2 { get; set; }
    }
    private class ClassTimeComparer : IComparer<ClassTimes>
    {
        public int Compare(ClassTimes x, ClassTimes y)
        {
            return x.ToString().CompareTo(y.ToString());
        }
    }
    static bool AreOnSameDay(ClassTimes classTime1, ClassTimes classTime2)
    {
        // Extract day from enum values
        var day1 = classTime1.ToString().Substring(0, classTime1.ToString().IndexOf('T'));
        var day2 = classTime2.ToString().Substring(0, classTime2.ToString().IndexOf('T'));

        // Compare days
        return day1 == day2;
    }
    #endregion

    #region Penalty
    public int CalculatePenalty(Schedule schedule)
    {
        var penalty = 0;
        penalty += PenaltyOfOverlay(schedule.CourseTeacherClassTimes);
        penalty += PenaltyOfTeacher(schedule.CourseTeacherClassTimes);
        return penalty;
    }
    private int PenaltyOfTeacher(List<CourseTeacherClassTime> CourseTeacherClassTimes)
    {
        var penalties = new Dictionary<Teacher, int>();

        foreach (var course in CourseTeacherClassTimes)
        {
            var teacher = course.Teacher;
            if (!penalties.ContainsKey(teacher))
            {
                penalties[teacher] = 0;
            }

            var classTimes = course.ClassTime.Select(ct => ct.ClassTime).OrderBy(ct => ct).ToList();
            for (int i = 0; i < classTimes.Count - 1; i++)
            {
                var currentClass = classTimes[i];
                var nextClass = classTimes[i + 1];

                // Check if classes are not consecutive and on the same day
                if (nextClass - currentClass > 1 && AreOnSameDay(currentClass, nextClass))
                {
                    penalties[teacher]++;
                }
            }
        }

        var totalPenalty = 0;
        foreach (var item in penalties)
        {
            totalPenalty += item.Value;
        }
        return totalPenalty;
    }
    private int PenaltyOfOverlay(List<CourseTeacherClassTime> CourseTeacherClassTimes)
    {
        var penalty = 0;
        var calculatedCoursePenalty = new List<CalculatedCoursePenalty>();
        foreach (var CTT1 in CourseTeacherClassTimes)
        {
            foreach (var CTT2 in CourseTeacherClassTimes)
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
    #endregion

    public async Task<List<Schedule>> GeneratePopulation(int Count = 100)
    {
        var schedules = new List<Schedule>();
        var count = 0;
        var unhealtyCount = 0;
        while (count < Count)
        {
            if (unhealtyCount > Count * 10)
            {
                break;
            }
            #region local var

            var schedule = new Schedule();
            bool isHealthy = true;
            var courentCourseList = new List<Course>();
            courentCourseList.AddRange(Courses);
            var courentTeacherList = new List<CoustomTeacherWithEvenOddClassTime>();
            foreach (var techer in Teachers)
            {
                var t = new CoustomTeacherWithEvenOddClassTime()
                {
                    ID = techer.ID,
                    Name = techer.Name,
                };
                foreach (var time in techer.PreferredTimes)
                {
                    var coustomEvenOdd = new List<CoustomEvenOdd>
                    {
                        CoustomEvenOdd.everyWeek
                    };
                    t.PreferredTime.Add(new()
                    {
                        //ClassTime = time,
                        EvenOdd = coustomEvenOdd,
                    });
                }
                courentTeacherList.Add(t);
            }
            #endregion
            for (int j = 1; j <= Courses.Count(); j++)
            {

                var CTT = new CourseTeacherClassTime();
                CTT.Course = SelectCourse(courentCourseList);

                var teacherOfThisCourse = CourseToTeacher.Where(ct => ct.CourseID == CTT.Course.ID).ToList();

                var rnd = new Random();
                for (int i = 0; i < CTT.Course.CountOfClass; i++)
                {
                    while (true)
                    {
                        if (teacherOfThisCourse.Count() != 0)
                        {
                            var teacherId = teacherOfThisCourse[rnd.Next(teacherOfThisCourse.Count())].TeacherID;
                            var teacher = courentTeacherList.First(t => t.ID == teacherId);
                            var countOfFreetimeOfTeacherEveryWeek = teacher.PreferredTime.Where(t => t.EvenOdd.Contains(CoustomEvenOdd.everyWeek)).Count();
                            var countOfFreeTimeOfTecherEvenOdd = teacher.PreferredTime.Where(t => t.EvenOdd.Contains(CoustomEvenOdd.everyWeek)).Count() * 2 +
                                teacher.PreferredTime.Where(t => t.EvenOdd.Contains(CoustomEvenOdd.even)).Count() +
                                teacher.PreferredTime.Where(t => t.EvenOdd.Contains(CoustomEvenOdd.odd)).Count();
                            var countOfClassNeedInEveryWeek = CTT.Course.Credits / 2;

                            var thisTeacherCanTakeCourse = false;
                            if (countOfFreetimeOfTeacherEveryWeek >= countOfClassNeedInEveryWeek)
                            {
                                if (CTT.Course.Credits == 3)
                                {
                                    if (countOfFreeTimeOfTecherEvenOdd - 2 >= 1)
                                    {
                                        thisTeacherCanTakeCourse = true;
                                    }
                                }
                                else
                                {
                                    thisTeacherCanTakeCourse = true;
                                }
                            }
                            if (thisTeacherCanTakeCourse)
                            {
                                var courseCredits = CTT.Course.Credits;
                                while (courseCredits > 0)
                                {
                                    var time = teacher.PreferredTime[rnd.Next(teacher.PreferredTime.Count())];
                                    if (time.EvenOdd.Count() != 0)
                                    {
                                        CTT.Teacher = Teachers.First(t => t.ID == teacherId);
                                        if (courseCredits == 1)
                                        {
                                            if (time.EvenOdd.Contains(CoustomEvenOdd.everyWeek))
                                            {
                                                if (rnd.NextDouble() > 0.5)
                                                {
                                                    CTT.ClassTime.Add(new()
                                                    {
                                                        ClassTime = time.ClassTime,
                                                        EvenOdd = EvenOdd.odd
                                                    });
                                                    courentTeacherList.First(t => t == teacher).PreferredTime.First(t => t.ClassTime == time.ClassTime).EvenOdd.Clear();
                                                    courentTeacherList.First(t => t == teacher).PreferredTime.First(t => t.ClassTime == time.ClassTime).EvenOdd.Add(CoustomEvenOdd.even);
                                                }
                                                else
                                                {
                                                    CTT.ClassTime.Add(new()
                                                    {
                                                        ClassTime = time.ClassTime,
                                                        EvenOdd = EvenOdd.even
                                                    });
                                                    courentTeacherList.First(t => t == teacher).PreferredTime.First(t => t.ClassTime == time.ClassTime).EvenOdd.Clear();
                                                    courentTeacherList.First(t => t == teacher).PreferredTime.First(t => t.ClassTime == time.ClassTime).EvenOdd.Add(CoustomEvenOdd.odd);
                                                }
                                                courseCredits -= 1;
                                            }
                                            else if (time.EvenOdd.Contains(CoustomEvenOdd.odd))
                                            {
                                                CTT.ClassTime.Add(new()
                                                {
                                                    ClassTime = time.ClassTime,
                                                    EvenOdd = EvenOdd.odd
                                                });
                                                courentTeacherList.First(t => t == teacher).PreferredTime.First(t => t.ClassTime == time.ClassTime).EvenOdd.Clear();
                                                courseCredits -= 1;

                                            }
                                            else if (time.EvenOdd.Contains(CoustomEvenOdd.even))
                                            {
                                                CTT.ClassTime.Add(new()
                                                {
                                                    ClassTime = time.ClassTime,
                                                    EvenOdd = EvenOdd.even
                                                });
                                                courentTeacherList.First(t => t == teacher).PreferredTime.First(t => t.ClassTime == time.ClassTime).EvenOdd.Clear();
                                                courseCredits -= 1;
                                            }
                                        }
                                        else
                                        {
                                            if (time.EvenOdd.Contains(CoustomEvenOdd.everyWeek))
                                            {
                                                CTT.ClassTime.Add(new()
                                                {
                                                    ClassTime = time.ClassTime,
                                                    EvenOdd = EvenOdd.everyWeek
                                                });
                                                teacher.PreferredTime.Remove(time);
                                                courseCredits -= 2;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        teacher.PreferredTime.Remove(time);
                                    }
                                }
                                break;
                            }
                            else
                            {
                                teacherOfThisCourse.Remove(teacherOfThisCourse.First(tc => tc.TeacherID == teacherId));
                            }
                        }
                        else
                        {
                            isHealthy = false;
                            break;
                        }
                    }
                    schedule.CourseTeacherClassTimes.Add(CTT);
                }
            }
            if (isHealthy)
            {
                var penalty = CalculatePenalty(schedule);
                schedule.TotalPenalty = penalty;
                schedules.Add(schedule);
                count++;
            }
            else
            {
                unhealtyCount++;
            }
        }
        return schedules;
    }
    private Course SelectCourse(List<Course> courentCourseList)
    {
        var rnd = new Random();
        while (true)
        {
            var index = rnd.Next(Courses.Count());
            if (courentCourseList.Any(c => c == Courses[index]))
            {
                courentCourseList.Remove(Courses[index]);
                return Courses[index];
            }
        }
    }

}
