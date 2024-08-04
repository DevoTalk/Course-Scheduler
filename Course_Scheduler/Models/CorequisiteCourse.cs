namespace Course_Scheduler.Models
{
    public class CorequisiteCourse : Base
    {
        public int CourseId { get; set; }
        public int CorequisiteCourseId { get; set; }
        public Course Course { get; set; }



        public CorequisiteCourse DeepCopy()
        {
            return new CorequisiteCourse
            {
                CourseId = this.CourseId,
                CorequisiteCourseId = this.CorequisiteCourseId,
                Course = this.Course.DeepCopy()
            };
        }
    }
}