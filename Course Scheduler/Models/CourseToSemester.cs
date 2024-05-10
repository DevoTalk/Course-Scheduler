namespace Course_Scheduler.Models
{
    public class CourseToSemester : Base
    {
        public int CourseID { get; set; }
        public int SemesterID { get; set; }
        public Course Course { get; set; }
        public Semester semester { get; set; }
    }
}
