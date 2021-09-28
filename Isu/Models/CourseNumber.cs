using System.Collections.Generic;

namespace Isu.Models
{
    public class CourseNumber
    {
        public CourseNumber(int courseNumber)
        {
            Course = courseNumber;
        }

        public int Course{ get; }
    }
}