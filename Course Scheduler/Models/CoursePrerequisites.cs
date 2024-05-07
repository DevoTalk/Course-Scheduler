using System.ComponentModel.DataAnnotations.Schema;

namespace Course_Scheduler.Models
{
    public class CoursePrerequisites : Base
    {
        public int CourseId { get; set; }
        public int PrerequisiteCourseId { get; set; }
        public Course Course { get; set; }
    }
}
