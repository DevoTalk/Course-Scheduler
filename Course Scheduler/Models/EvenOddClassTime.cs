using Course_Scheduler.Models.Enum;

namespace Course_Scheduler.Models
{
    public class EvenOddClassTime
    {
        public ClassTimes ClassTime { get; set; }
        public EvenOdd? EvenOdd { get; set; }
    }
    public enum EvenOdd
    {
        even,
        odd,
        everyWeek
    }
}