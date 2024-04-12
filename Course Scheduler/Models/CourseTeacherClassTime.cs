namespace Course_Scheduler.Models
{
    public class CourseTeacherClassTime : Base
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
