using System.ComponentModel.DataAnnotations;

namespace Course_Scheduler.Models
{
    public class Course : Base
    {
        [Key]
        public int ID { get; set; }
        
        public string Name { get; set; }
        
        public Course? Required { get; set; }

        
    }
}
