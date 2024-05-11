namespace Course_Scheduler.Models.ViewModels
{
    public class AddCourseViewModel
    {
        public AddCourseViewModel()
        {
            this.TeachersId = new();
            this.PrerequisitesId = new();
            this.CorequisitesId = new();
        }
        public Course Course { get; set; }
        public List<int> TeachersId { get; set; }
        public List<int>? PrerequisitesId { get; set; }
        public List<int>? CorequisitesId{ get; set; }
    }
}
