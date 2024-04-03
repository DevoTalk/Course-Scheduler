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
        public int TotalPenalty { get; set; }
    }
    public class CourseTeacherClassTime
    {
        public CourseTeacherClassTime()
        {
            this.ClassTime = new();
        }
        public Course Course { get; set; }
        public Teacher Teacher { get; set; }
        public List<EvenOddClassTime> ClassTime { get; set; }

    }
    
}
