using System.ComponentModel.DataAnnotations;

namespace Course_Scheduler.Models
{
    public class Course : Base
    {
        [Key]
        public int ID { get; set; }
        
        public string Name { get; set; }

        public int? RequiredCourseID { get; set; }
        public Course? RequiredCourse { get; set; }
        
        
    }
}
