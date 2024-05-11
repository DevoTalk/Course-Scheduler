using System.ComponentModel.DataAnnotations;

namespace Course_Scheduler.Models
{
    public class Course : Base
    {

        public Course()
        {
            this.Prerequisites = new();
            this.CorequisiteCourses = new();
        }

        public string Name { get; set; }
        public int Credits { get; set; } = 3;
        public int CountOfClass { get; set; } = 1;
        public string CourseCode { get; set; }


        public List<CorequisiteCourse> CorequisiteCourses { get; set; }
        public List<CoursePrerequisites> Prerequisites { get; set; }
    }
}
