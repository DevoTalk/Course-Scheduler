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
        public Course Course { get; set; }
        public Teacher Teacher { get; set; }
        public ClassTime ClassTime { get; set; }
    }
}
