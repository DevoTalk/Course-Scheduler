namespace Course_Scheduler.Models
{
    public class CourseToGroups:Base
    {
        public int CourseId { get; set; }
        public int GroupId { get; set; }
        public Course Course { get; set; }
        public Group Group { get; set; }
    }
}
