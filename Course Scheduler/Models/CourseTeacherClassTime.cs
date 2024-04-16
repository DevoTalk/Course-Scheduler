namespace Course_Scheduler.Models
{
    public class CourseTeacherClassTime : Base
    {

        public CourseTeacherClassTime()
        {
            this.ClassTimes = new();
        }

        public Course Course { get; set; }
        public Teacher Teacher { get; set; }
        public List<EvenOddClassTime> ClassTimes { get; set; }

    }
}
