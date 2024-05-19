using Course_Scheduler.Models.Enum;

namespace Course_Scheduler.Models
{

    public class Schedule
    {
        public Schedule()
        {
            this.CourseTeacherClassTimes = new();
        }
        public List<CourseTeacherClassTime> CourseTeacherClassTimes { get; set; }
        public Penalty Penalty { get; set; }




        public Schedule DeepCopy()
        {
            var newSchedule = new Schedule
            {
                Penalty = new Penalty
                {
                    TotalPenalty = this.Penalty?.TotalPenalty ?? 0,
                    PenaltyOfOverlay = this.Penalty?.PenaltyOfOverlay ?? 0,
                    PenaltyOfTeacher = this.Penalty?.PenaltyOfTeacher ?? 0,
                    PenaltyOfMaximumCountOfClassInSection = this.Penalty?.PenaltyOfMaximumCountOfClassInSection ?? 0
                }
            };

            foreach (var ctt in this.CourseTeacherClassTimes)
            {
                newSchedule.CourseTeacherClassTimes.Add(new CourseTeacherClassTime
                {
                    Course = ctt.Course.DeepCopy(),
                    Teacher = ctt.Teacher.DeepCopy(),
                    SemesterId = ctt.SemesterId,
                    ClassTimes = ctt.ClassTimes.Select(ct => new EvenOddClassTime
                    {
                        ClassTime = ct.ClassTime,
                        EvenOdd = ct.EvenOdd,
                        CourseTeacherClassTimeId = ct.CourseTeacherClassTimeId
                    }).ToList()
                });
            }

            return newSchedule;
        }
    }
}
