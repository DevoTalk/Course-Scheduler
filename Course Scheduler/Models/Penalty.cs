namespace Course_Scheduler.Models
{
    public class Penalty
    {
        public int TotalPenalty { get; set; }
        public int PenaltyOfOverlay { get; set; }
        public int PenaltyOfTeacher { get; set; }
        public int PenaltyOfMaximumCountOfClassInSection { get; set; }
    }
}