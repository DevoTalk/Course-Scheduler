namespace Course_Scheduler.Models
{
    public class CorequisiteCourse : Base
    {
        public int CourseId { get; set; }
        public int CorequisiteCourseId { get; set; }
        public Course Course { get; set; }

    }
}