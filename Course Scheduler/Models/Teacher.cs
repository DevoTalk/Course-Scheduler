using Course_Scheduler.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace Course_Scheduler.Models;

public class Teacher : Base
{
    public Teacher()
    {
        PreferredTimes = new List<TeacherClassTimeWithPenalties>();
    }
    public string Name { get; set; }
    public List<TeacherClassTimeWithPenalties> PreferredTimes{ get; set; }
}
public class TeacherClassTimeWithPenalties : Base
{
    public ClassTimes PreferredTime {get; set; }
    public int Penalty {get; set; }
    public int TeacherId { get; set; }
    public Teacher Teacher { get; set; }
}
