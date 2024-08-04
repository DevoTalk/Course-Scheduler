namespace Course_Scheduler.Models.ViewModels
{
    public class AddSemesterViewModel
    {
        public AddSemesterViewModel()
        {
            this.CoursesId = new();
        }
        public int? ID { get; set; }
        public string Name { get; set; } = "";
        public List<int> CoursesId { get; set; }
    }
}
