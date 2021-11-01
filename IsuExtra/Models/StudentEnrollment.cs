using System;

namespace IsuExtra.Models
{
    public class StudentEnrollment
    {
        public StudentEnrollment(Student student)
        {
            Student = student;
            if (Student == null)
            {
                throw new NullReferenceException(nameof(Student.Name));
            }
        }

        public Student Student { get; }
    }
}