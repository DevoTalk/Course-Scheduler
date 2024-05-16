namespace Course_Scheduler.Models
{
    public class Group : Base
    {
        public Group()
        {
            this.Courses = new();
        }
        public string Name { get; set; }
        public List<Course> Courses { get; set; }
    }
}
