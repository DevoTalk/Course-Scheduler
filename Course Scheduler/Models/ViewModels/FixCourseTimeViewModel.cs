using Course_Scheduler.Models.Enum;

namespace Course_Scheduler.Models.ViewModels
{
    public class FixCourseTimeViewModel
    {
        public FixCourseTimeViewModel()
        {
            this.Times = new();
            this.Teachers = new();
        }
        public int CourseId { get; set; }
        public int TeacherId { get; set; }
        public int CourseCredits { get; set; }

        public List<Teacher> Teachers { get; set; }
        public List<EvenOddClassTimeViewModel> Times { get; set; }
    }
    public class EvenOddClassTimeViewModel
    {
        public ClassTimes? ClassTime { get; set; }
        public EvenOdd? EvenOdd { get; set; }
    }
}
