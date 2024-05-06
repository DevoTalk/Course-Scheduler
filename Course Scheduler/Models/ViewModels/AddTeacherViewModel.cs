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
        public int MaximumDayCount { get; set; }
        public int PenaltyForEmptyTime { get; set; } = 1;


        public List<TeacherClassTimeWithPenaltiesViewModel> PreferredTime { get; set; }
    }
    public class TeacherClassTimeWithPenaltiesViewModel
    {
        public ClassTimes PreferredTime { get; set; }
        public int Penalty { get; set; }
    }
}
