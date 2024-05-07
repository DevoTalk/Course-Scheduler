using System.ComponentModel.DataAnnotations;

namespace Course_Scheduler.Models
{
    public class Course : Base
    {

        public Course()
        {
            this.Prerequisites = new();
        }

        public string Name { get; set; }
        public int? PrerequisiteID { get; set; }
        public Course? Prerequisite { get; set; }
        public int Credits { get; set; } = 3;
        public int CountOfClass { get; set; } = 1;



        public List<CoursePrerequisites> Prerequisites { get; set; }
    }
}
