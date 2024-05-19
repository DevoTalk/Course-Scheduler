using System.ComponentModel.DataAnnotations;

namespace Course_Scheduler.Models
{
    public class Course : Base
    {

        public Course()
        {
            this.Prerequisites = new();
            this.CorequisiteCourses = new();
            this.Groups = new();
        }

        public string Name { get; set; }
        public int Credits { get; set; } = 3;
        public int CountOfClass { get; set; } = 1;
        public string CourseCode { get; set; }


        public List<Group> Groups { get; set; }
        public List<CorequisiteCourse> CorequisiteCourses { get; set; }
        public List<CoursePrerequisites> Prerequisites { get; set; }





        public Course DeepCopy()
        {
            var newCourse = new Course
            {
                Name = this.Name,
                Credits = this.Credits,
                CountOfClass = this.CountOfClass,
                CourseCode = this.CourseCode,
                Groups = this.Groups.Select(g => g.DeepCopy()).ToList(),
                CorequisiteCourses = this.CorequisiteCourses.Select(cc => cc.DeepCopy()).ToList(),
                Prerequisites = this.Prerequisites.Select(cp => cp.DeepCopy()).ToList()
            };

            return newCourse;
        }
    }
}
