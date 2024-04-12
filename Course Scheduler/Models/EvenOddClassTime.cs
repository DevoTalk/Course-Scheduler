using Course_Scheduler.Models.Enum;

namespace Course_Scheduler.Models
{
    public class EvenOddClassTime : Base
    {
        public ClassTimes ClassTime { get; set; }
        public EvenOdd? EvenOdd { get; set; }
        public int CourseTeacherClassTimeId { get; set; }
        public CourseTeacherClassTime CourseTeacherClass { get; set; }
    }
}