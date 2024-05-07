namespace Course_Scheduler.Models.ViewModels
{
    public class AddCourseViewModel
    {
        public Course Course { get; set; }
        public List<int> TeachersId { get; set; }
        public List<int> PrerequisitesId { get; set; }
    }
}
