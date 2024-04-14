namespace Course_Scheduler.Models.ViewModels;

public class UpdateTeacherViewModel:Base
{
    public UpdateTeacherViewModel()
    {
        this.PreferredTime = new();
    }
    public string Name { get; set; }
    public int MaximumDayCount { get; set; }

    public List<TeacherClassTimeWithPenaltiesViewModel> PreferredTime { get; set; }
}
