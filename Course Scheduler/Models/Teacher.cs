using Course_Scheduler.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace Course_Scheduler.Models
{
    public class Teacher : Base
    {
        public Teacher()
        {
            PreferredTime = new List<ClassTime>();
        }
        public string Name { get; set; }
        public List<ClassTime> PreferredTime{ get; set; }
    }
}
