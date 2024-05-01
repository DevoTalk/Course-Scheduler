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
    }
}
