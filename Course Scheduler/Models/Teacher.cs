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
    public string TeacherCode { get; set; }
    public List<TeacherClassTimeWithPenalties> PreferredTimes{ get; set; }
    public int MaximumDayCount { get; set; }
    public int PenaltyForEmptyTime { get; set; } = 1;

    public Teacher DeepCopy()
    {
        return new Teacher
        {
            Name = this.Name,
            TeacherCode = this.TeacherCode,
            MaximumDayCount = this.MaximumDayCount,
            PenaltyForEmptyTime = this.PenaltyForEmptyTime,
            PreferredTimes = this.PreferredTimes.Select(pt => pt.DeepCopy()).ToList()
        };
    }

}
public class TeacherClassTimeWithPenalties : Base
{
    public ClassTimes PreferredTime {get; set; }
    public int Penalty {get; set; }
    public EvenOdd? EvenOdd { get; set; }
    public int TeacherId { get; set; }
    public Teacher Teacher { get; set; }


    public TeacherClassTimeWithPenalties DeepCopy()
    {
        return new TeacherClassTimeWithPenalties
        {
            PreferredTime = this.PreferredTime,
            Penalty = this.Penalty,
            EvenOdd = this.EvenOdd,
            TeacherId = this.TeacherId,
            Teacher = this.Teacher?.DeepCopy()
        };
    }

}
