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



        public Group DeepCopy()
        {
            return new Group
            {
                Name = this.Name,
                Courses = this.Courses.Select(c => c.DeepCopy()).ToList()
            };
        }
    }
}
