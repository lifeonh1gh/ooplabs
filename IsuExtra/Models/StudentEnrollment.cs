using System;

namespace IsuExtra.Models
{
    public class StudentEnrollment
    {
        public StudentEnrollment(Student student)
        {
            Student = student ?? throw new NullReferenceException(nameof(Student.Name));
        }
        public Student Student { get; }
    }
}