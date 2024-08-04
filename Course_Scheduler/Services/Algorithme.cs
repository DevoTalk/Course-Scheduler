using Course_Scheduler.Models;
using Course_Scheduler.Models.Enum;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Course_Scheduler.Services
{
    public class Algorithme
    {
        public List<Course> Courses { get; }
        public List<CourseToTeacher> CourseToTeacher { get; }
        public List<CoursePenalty> CoursePenalties { get; }
        public List<Teacher> Teachers { get; }
        public List<CourseTeacherClassTime> FixedCourses { get; }
        public List<CoursePrerequisites> CoursePrerequisites { get; }

        public Algorithme(
            List<Course> courses,
            List<CourseToTeacher> courseToTeacher,
            List<CoursePenalty> coursePenalties,
            List<Teacher> teachers,
            List<CourseTeacherClassTime> fixedCourses,
            List<CoursePrerequisites> coursePrerequisites)
        {
            Courses = courses;
            CourseToTeacher = courseToTeacher;
            CoursePenalties = coursePenalties;
            Teachers = teachers;
            FixedCourses = fixedCourses;
            CoursePrerequisites = coursePrerequisites;

            foreach (var teacher in Teachers)
            {
                foreach (var time in teacher.PreferredTimes)
                {
                    time.EvenOdd = EvenOdd.everyWeek;
                }
            }
        }

        private static bool AreOnSameDay(ClassTimes classTime1, ClassTimes classTime2)
        {
            var day1 = classTime1.ToString().Split('T')[0];
            var day2 = classTime2.ToString().Split('T')[0];
            return day1 == day2;
        }

        public Penalty CalculatePenalty(Schedule schedule)
        {
            var penalty = new Penalty
            {
                PenaltyOfOverlay = PenaltyOfOverlay(schedule),
                PenaltyOfTeacher = TotallPenaltyOfTeachers(schedule),
                PenaltyOfMaximumCountOfClassInSection = PenaltyOfMaximumCountOfClassInSection(schedule),
            };
            penalty.TotalPenalty = penalty.PenaltyOfOverlay + penalty.PenaltyOfTeacher + penalty.PenaltyOfMaximumCountOfClassInSection;
            return penalty;
        }

        private int PenaltyOfMaximumCountOfClassInSection(Schedule schedule)
        {
            const int maximumCountOfClass = 3;
            var sections = schedule.CourseTeacherClassTimes
                .SelectMany(ctt => ctt.ClassTimes)
                .GroupBy(t => t.ClassTime)
                .Where(g => g.Count() > maximumCountOfClass)
                .Sum(g => (g.Count() - maximumCountOfClass) * 2);

            return sections;
        }

        private int TotallPenaltyOfTeachers(Schedule schedule)
        {
            return PenaltyOfTeachers(schedule).Values.Sum();
        }

        private Dictionary<Teacher, int> PenaltyOfTeachers(Schedule schedule)
        {
            var penalties = new Dictionary<Teacher, int>();

            foreach (var courseTeacherClassTime in schedule.CourseTeacherClassTimes)
            {
                var teacher = courseTeacherClassTime.Teacher;

                if (!penalties.ContainsKey(teacher))
                {
                    penalties[teacher] = 0;
                }

                var classTimes = courseTeacherClassTime.ClassTimes.Select(ct => ct.ClassTime).OrderBy(ct => ct).ToList();
                penalties[teacher] += CalculatePenaltyForTeacher(teacher, classTimes);
            }

            return penalties;
        }

        private int CalculatePenaltyForTeacher(Teacher teacher, List<ClassTimes> classTimes)
        {
            int penalty = 0;
            for (int i = 0; i < classTimes.Count - 1; i++)
            {
                var currentClass = classTimes[i];
                var nextClass = classTimes[i + 1];

                if ((nextClass - currentClass) > 1 && AreOnSameDay(currentClass, nextClass))
                {
                    penalty += teacher.PenaltyForEmptyTime;
                }
            }

            foreach (var classTime in classTimes)
            {
                penalty += teacher.PreferredTimes.FirstOrDefault(t => t.PreferredTime == classTime)?.Penalty ?? 0;
            }

            return penalty;
        }

        private int PenaltyOfOverlay(Schedule schedule)
        {
            var penalty = 0;
            var calculatedCoursePenalty = new HashSet<(Course, Course)>();

            foreach (var ctt1 in schedule.CourseTeacherClassTimes)
            {
                foreach (var ctt2 in schedule.CourseTeacherClassTimes)
                {
                    if (ctt1 == ctt2 || !ctt1.Course.Groups.Intersect(ctt2.Course.Groups).Any()) continue;

                    foreach (var ct1 in ctt1.ClassTimes)
                    {
                        foreach (var ct2 in ctt2.ClassTimes)
                        {
                            if (ct1.ClassTime != ct2.ClassTime ||
                                !(ct1.EvenOdd == ct2.EvenOdd || ct1.EvenOdd == EvenOdd.everyWeek || ct2.EvenOdd == EvenOdd.everyWeek)) continue;

                            var coursePair = (ctt1.Course, ctt2.Course);
                            var reverseCoursePair = (ctt2.Course, ctt1.Course);

                            if (!calculatedCoursePenalty.Contains(coursePair) && !calculatedCoursePenalty.Contains(reverseCoursePair))
                            {
                                penalty += CoursePenalties
                                    .FirstOrDefault(c => (c.CourseID == ctt1.Course.ID && c.CourseWithPenaltyID == ctt2.Course.ID) ||
                                                         (c.CourseID == ctt2.Course.ID && c.CourseWithPenaltyID == ctt1.Course.ID))?.PenaltyCount ?? 0;
                                calculatedCoursePenalty.Add(coursePair);
                            }
                        }
                    }
                }
            }

            return penalty;
        }

        public Schedule DeepCopy(Schedule schedule)
        {
            var json = JsonSerializer.Serialize(schedule);
            return JsonSerializer.Deserialize<Schedule>(json);
        }

        public async Task<List<Schedule>> CreateSchedules(int count = 100, int unhealthyCount = 1000)
        {
            var allSchedules = new ConcurrentBag<Schedule>();

            var tasks = Enumerable.Range(0, 10).Select(async _ =>
            {
                var schedules = GeneratePopulation(count, unhealthyCount).Take(10);
                foreach (var schedule in schedules)
                {
                    var optimizedSchedule = await Task.Run(() => Optimizer(schedule));
                    allSchedules.Add(optimizedSchedule);
                }
            });

            await Task.WhenAll(tasks);

            return allSchedules.Distinct().ToList();
        }

        public List<Schedule> GeneratePopulation(int count = 100, int UnhealthyCount = 1000)
        {
            var schedules = new List<Schedule>();
            var unhealthyCount = 0;

            while (schedules.Count < count && unhealthyCount > UnhealthyCount)
            {
                var schedule = new Schedule();
                var courentCourseList = new HashSet<Course>(Courses);
                var courentTeachers = Teachers.Select(t => new Teacher
                {
                    ID = t.ID,
                    Name = t.Name,
                    MaximumDayCount = t.MaximumDayCount,
                    PreferredTimes = new List<TeacherClassTimeWithPenalties>(t.PreferredTimes.Select(p => new TeacherClassTimeWithPenalties
                    {
                        ID = p.ID,
                        EvenOdd = p.EvenOdd,
                        Penalty = p.Penalty,
                        PreferredTime = p.PreferredTime,
                        Teacher = p.Teacher,
                        TeacherId = p.TeacherId
                    }))
                }).ToArray();

                schedule = AddFixedCourseToSchedule(schedule, courentCourseList, courentTeachers);
                var isHealthy = true;

                foreach (var course in courentCourseList.ToList())
                {
                    var ctt = new CourseTeacherClassTime { Course = course };
                    var teachersOfThisCourse = CourseToTeacher.Where(ct => ct.CourseID == ctt.Course.ID).ToList();
                    var teacher = SelectTeacherForCourse(ctt.Course, teachersOfThisCourse, courentTeachers);

                    if (teacher == null)
                    {
                        isHealthy = false;
                        break;
                    }

                    ctt.Teacher = teacher;
                    AssignClassTimesToCourse(ctt, teacher);

                    if (ctt.ClassTimes.Count == 0)
                    {
                        isHealthy = false;
                        break;
                    }

                    schedule.CourseTeacherClassTimes.Add(ctt);
                }

                if (isHealthy && AreRequirementsMet(schedule))
                {
                    schedule.Penalty = CalculatePenalty(schedule);
                    schedules.Add(schedule);
                }
                else
                {
                    unhealthyCount++;
                }
            }

            return schedules;
        }

        private Schedule AddFixedCourseToSchedule(Schedule schedule, HashSet<Course> courentCourseList, Teacher[] courentTeachers)
        {
            foreach (var fixedCourse in FixedCourses)
            {
                courentCourseList.RemoveWhere(c => c.ID == fixedCourse.Course.ID);
                schedule.CourseTeacherClassTimes.Add(new CourseTeacherClassTime
                {
                    Course = fixedCourse.Course,
                    //ClassTimes = new List<CourseTeacherClassTime.ClassTimes>(fixedCourse.ClassTimes.Select(ct => new CourseTeacherClassTime.ClassTimes
                    //{
                    //    ClassTime = ct.ClassTime,
                    //    EvenOdd = ct.EvenOdd
                    //})),
                    Teacher = courentTeachers.First(t => t.ID == fixedCourse.Teacher.ID)
                });
            }

            return schedule;
        }

        private Teacher SelectTeacherForCourse(Course course, List<CourseToTeacher> teachersOfThisCourse, Teacher[] courentTeachers)
        {
            var bestTeacher = courentTeachers
                .Where(t => teachersOfThisCourse.Any(tc => tc.TeacherID == t.ID))
                .OrderBy(t => t.PreferredTimes.Count)
                .FirstOrDefault();

            return bestTeacher;
        }

        private void AssignClassTimesToCourse(CourseTeacherClassTime ctt, Teacher teacher)
        {
            //var courseClassTimes = new List<CourseTeacherClassTime.ClassTimes>();
            //var teacherPreferredTimes = teacher.PreferredTimes.Select(t => t.PreferredTime).ToList();

            //while (courseClassTimes.Count < 2 && teacherPreferredTimes.Count > 0)
            //{
            //    var randomClassTime = teacherPreferredTimes[new Random().Next(teacherPreferredTimes.Count)];
            //    teacherPreferredTimes.Remove(randomClassTime);

            //    if (!courseClassTimes.Any(ct => ct.ClassTime == randomClassTime))
            //    {
                    //courseClassTimes.Add(new CourseTeacherClassTime.ClassTimes { ClassTime = randomClassTime, EvenOdd = EvenOdd.everyWeek });
               // }
            //}

            //ctt.ClassTimes = courseClassTimes;
        }

        private bool AreRequirementsMet(Schedule schedule)
        {
            // Check all requirements here...
            return true;
        }

        public Schedule Optimizer(Schedule schedule)
        {
            var newSchedule = DeepCopy(schedule);

            foreach (var ctt in newSchedule.CourseTeacherClassTimes)
            {
                var penalties = PenaltyOfTeachers(newSchedule);
                var worstTeacher = penalties.OrderByDescending(p => p.Value).FirstOrDefault().Key;

                if (worstTeacher != null)
                {
                    var currentTimes = ctt.ClassTimes.Select(ct => ct.ClassTime).ToList();
                    var teacherTimes = worstTeacher.PreferredTimes.Select(t => t.PreferredTime).ToList();
                    var availableTimes = teacherTimes.Except(currentTimes).ToList();

                    if (availableTimes.Any())
                    {
                        var newClassTime = availableTimes[new Random().Next(availableTimes.Count)];
                        ctt.ClassTimes[0].ClassTime = newClassTime;
                    }
                }
            }

            return newSchedule;
        }
    }
}

