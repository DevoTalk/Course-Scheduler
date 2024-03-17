using Course_Scheduler.Models.Enum;

namespace Course_Scheduler.Models.ViewModels
{
    public class AddTeacherViewModel
    {
        public AddTeacherViewModel()
        {
            this.PreferredTime = new();
        }
        public string Name { get; set; }
        public List<ClassTime> PreferredTime { get; set; }
    }
}
