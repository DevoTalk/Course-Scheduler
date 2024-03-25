using System.ComponentModel.DataAnnotations;

namespace Course_Scheduler.Models
{
    public class Course : Base
    {
        public string Name { get; set; }
        public int? PrerequisiteID { get; set; }
        public Course? Prerequisite { get; set; }
        public int Credits { get; set; }
    }
}
