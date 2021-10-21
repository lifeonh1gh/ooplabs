using System;

namespace IsuExtra.Models
{
    public class StudentUnsigned
    {
        public StudentUnsigned(Student student)
        {
            Student = student ?? throw new NullReferenceException(nameof(Student.Name));
        }

        public Student Student { get; }
    }
}