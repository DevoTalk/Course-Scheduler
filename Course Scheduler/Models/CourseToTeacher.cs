using Course_Scheduler.Models.Enum;

namespace Course_Scheduler.Models
{
    public class CourseToTeacher : Base
    {
        public int CourseID { get; set; }
        public Course Course { get; set; }

        public int TeacherID { get; set; }
        public Teacher Teacher { get; set; }


        public ClassTime ClassTime { get; set; }
    }
}
