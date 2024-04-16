using Course_Scheduler.Models;

namespace Course_Scheduler.Services;

public static class ScheduleExtensions
{
    public static List<Schedule> Distinct(this List<Schedule> schedules)
    {
        // Create a HashSet of schedules using the ScheduleComparer
        HashSet<Schedule> uniqueSchedules = new HashSet<Schedule>(new ScheduleComparer());

        // Add schedules to the HashSet
        foreach (var schedule in schedules)
        {
            uniqueSchedules.Add(schedule);
        }

        // Convert the HashSet back to a List
        return uniqueSchedules.ToList();
    }
}

public class ScheduleComparer: IEqualityComparer<Schedule>
{
    public bool Equals(Schedule x, Schedule y)
    {
        //Check whether the compared objects reference the same data.
        if (Object.ReferenceEquals(x, y)) return true;

        //Check whether any of the compared objects is null.
        if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
            return false;


        // Check equality of CourseTeacherClassTimes
        if (x.CourseTeacherClassTimes.Count != y.CourseTeacherClassTimes.Count)
            return false;

        for (int i = 0; i < x.CourseTeacherClassTimes.Count; i++)
        {
            var cttX = x.CourseTeacherClassTimes[i];
            var cttY = y.CourseTeacherClassTimes[i];

            // Check equality of Course and Teacher names
            if (cttX.Course.Name != cttY.Course.Name || cttX.Teacher.Name != cttY.Teacher.Name)
                return false;

            // Check equality of ClassTime lists
            if (cttX.ClassTimes.Count != cttY.ClassTimes.Count)
                return false;

            // Check equality of each ClassTime
            foreach (var timeOfx in cttX.ClassTimes)
            {
                if (!cttY.ClassTimes.Any(timeOfy => timeOfy.ClassTime == timeOfx.ClassTime && timeOfy.EvenOdd == timeOfx.EvenOdd))
                    return false;
            }
        }

        return true;
    }
    public int GetHashCode(Schedule obj)
    {
        unchecked // Overflow is fine for this purpose
        {
            int hashCode = 17;

            // Include hash codes of each CourseTeacherClassTime
            foreach (var ctt in obj.CourseTeacherClassTimes)
            {
                hashCode = hashCode * 23 + ctt.Course.Name.GetHashCode();
                hashCode = hashCode * 23 + ctt.Teacher.Name.GetHashCode();
                foreach (var time in ctt.ClassTimes)
                {
                    hashCode = hashCode * 23 + time.ClassTime.GetHashCode();
                    hashCode = hashCode * 23 + time.EvenOdd.GetHashCode();
                }
            }

            return hashCode;
        }
    }
}

