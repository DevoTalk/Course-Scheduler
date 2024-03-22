using System.ComponentModel.DataAnnotations.Schema;

namespace Course_Scheduler.Models
{
    public class CoursePenalty : Base
    {
        public int CourseID { get; set; }
        public Course Course { get; set; }
        
        public int CourseWithPenaltyID { get; set; }

        public int PenaltyCount { get; set; } = 0;
    
    }
}
