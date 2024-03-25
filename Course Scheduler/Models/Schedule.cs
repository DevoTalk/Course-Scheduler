using Course_Scheduler.Models.Enum;

namespace Course_Scheduler.Models
{

    public class Schedule
    {
        public Schedule()
        {
            this.CourseTeacherClassTime = new();
        }
        public List<CourseTeacherClassTime> CourseTeacherClassTime { get; set; }
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
    public class EvenOddClassTime
    {
        public ClassTime ClassTime { get; set; }
        public EvenOdd? EvenOdd { get; set; }
    }
    public enum EvenOdd
    {
        even,
        odd, 
        everyWeek
    }
}
