using Course_Scheduler.Models;
using Course_Scheduler.Models.Enum;
using System.Collections.Generic;
using System.Text;

namespace Course_Scheduler.Services;

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
        foreach (var teacher in Teachers)
        {
            foreach (var time in teacher.PreferredTimes)
            {
                time.EvenOdd = EvenOdd.everyWeek;
            }
        }
    }
    #endregion
    #region ClassTimeComparer
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
        var totalPenalty = 0;

        foreach (var courseTeacherClassTime in CourseTeacherClassTimes)
        {
            var teacher = courseTeacherClassTime.Teacher;
            if (!penalties.ContainsKey(teacher))
            {
                penalties[teacher] = 0;
            }

            var classTimes = courseTeacherClassTime.ClassTime.Select(ct => ct.ClassTime).OrderBy(ct => ct).ToList();
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
            foreach (var classTime in classTimes)
            {
                var penaltieOfTime = Teachers.First(t => t == teacher).PreferredTimes.First(t => t.PreferredTime == classTime).Penalty;
                penalties[teacher] += penaltieOfTime;
            }
        }

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
            var courentTeacherList = new List<Teacher>();
            courentTeacherList.AddRange(Teachers);



            #endregion
            for (int j = 1; j <= Courses.Count(); j++)
            {

                var CTT = new CourseTeacherClassTime();
                CTT.Course = SelectCourse(courentCourseList);

                var teacherOfThisCourse = CourseToTeacher.Where(ct => ct.CourseID == CTT.Course.ID).ToList();

                var rnd = new Random();
                for (int i = 0; i < CTT.Course.CountOfClass; i++)
                {
                    var thisTeacherCanTakeCourse = false;
                    Teacher teacher = new();
                    while (!thisTeacherCanTakeCourse)
                    {
                        if (teacherOfThisCourse.Count() != 0)
                        {
                            var teacherId = teacherOfThisCourse[rnd.Next(teacherOfThisCourse.Count())].TeacherID;
                            teacher = courentTeacherList.First(t => t.ID == teacherId);

                            var countOfFreetimeOfTeacherEveryWeek = teacher.PreferredTimes.Where(t => t.EvenOdd == EvenOdd.everyWeek).Count();
                            var countOfFreeTimeOfTecherEvenOdd = teacher.PreferredTimes.Where(t => t.EvenOdd != EvenOdd.everyWeek).Count();

                            var countOfClassNeedInEveryWeek = CTT.Course.Credits / 2;


                            if (countOfFreetimeOfTeacherEveryWeek >= countOfClassNeedInEveryWeek)
                            {
                                if (CTT.Course.Credits == 3)
                                {
                                    if (countOfFreetimeOfTeacherEveryWeek -1 + countOfFreeTimeOfTecherEvenOdd > 0)
                                    {
                                        thisTeacherCanTakeCourse = true;
                                    }
                                }
                                else
                                {
                                    thisTeacherCanTakeCourse = true;
                                }
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
                    if (isHealthy)
                    {
                        CTT.Teacher = teacher;
                        var remainingCourseCredits = CTT.Course.Credits;
                        while (remainingCourseCredits > 0)
                        {
                            if (teacher.PreferredTimes.Count == 0)
                            {
                                isHealthy = false;
                                break;
                            }
                            var time = teacher.PreferredTimes[rnd.Next(teacher.PreferredTimes.Count())];
                            if (time.EvenOdd != null)
                            {
                                if (remainingCourseCredits == 1)
                                {
                                    if (time.EvenOdd == EvenOdd.everyWeek)
                                    {
                                        if (rnd.NextDouble() > 0.5)
                                        {
                                            CTT.ClassTime.Add(new()
                                            {
                                                ClassTime = time.PreferredTime,
                                                EvenOdd = EvenOdd.odd
                                            });
                                            courentTeacherList.First(t => t == teacher).PreferredTimes.First(t => t.PreferredTime == time.PreferredTime).EvenOdd = EvenOdd.even;
                                        }
                                        else
                                        {
                                            CTT.ClassTime.Add(new()
                                            {
                                                ClassTime = time.PreferredTime,
                                                EvenOdd = Models.Enum.EvenOdd.even
                                            });
                                            courentTeacherList.First(t => t == teacher).PreferredTimes.First(t => t.PreferredTime == time.PreferredTime).EvenOdd = EvenOdd.odd;
                                        }
                                        remainingCourseCredits -= 1;
                                    }
                                    else if (time.EvenOdd == EvenOdd.odd)
                                    {
                                        CTT.ClassTime.Add(new()
                                        {
                                            ClassTime = time.PreferredTime,
                                            EvenOdd = EvenOdd.odd
                                        });
                                        courentTeacherList.First(t => t == teacher).PreferredTimes.First(t => t.PreferredTime == time.PreferredTime).EvenOdd = null;
                                        remainingCourseCredits -= 1;
                                    }
                                    else if (time.EvenOdd == EvenOdd.even)
                                    {
                                        CTT.ClassTime.Add(new()
                                        {
                                            ClassTime = time.PreferredTime,
                                            EvenOdd = Models.Enum.EvenOdd.even
                                        });
                                        courentTeacherList.First(t => t == teacher).PreferredTimes.First(t => t.PreferredTime == time.PreferredTime).EvenOdd = null;
                                        remainingCourseCredits -= 1;
                                    }
                                }
                                else
                                {
                                    if (time.EvenOdd == EvenOdd.everyWeek)
                                    {
                                        CTT.ClassTime.Add(new()
                                        {
                                            ClassTime = time.PreferredTime,
                                            EvenOdd = EvenOdd.everyWeek
                                        });
                                        teacher.PreferredTimes.Remove(time);
                                        remainingCourseCredits -= 2;
                                    }
                                }
                            }
                            else
                            {
                                teacher.PreferredTimes.Remove(time);
                            }
                        }
                        schedule.CourseTeacherClassTimes.Add(CTT);
                    }
                }
            }
            if (isHealthy)
            {
                var penalty = CalculatePenalty(schedule);
                schedule.TotalPenalty = penalty;
                schedules.Add(schedule);
                schedules.Distinct();
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
            if (courentCourseList.Any(c => c.ID == Courses[index].ID))
            {
                courentCourseList.Remove(Courses[index]);
                return Courses[index];
            }
        }
    }
}
