namespace Course_Scheduler.Models.ViewModels
{
    public class CourseAndTeachersViewModel
    {
        public Course Course { get; set; }
        public List<Teacher> Teachers { get; set; }
        public bool IsFix { get; set; }
    }
}
