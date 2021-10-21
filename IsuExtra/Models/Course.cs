using System;

namespace IsuExtra.Models
{
    public class Course
    {
        public Course(int id, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Id = id;
            Name = name;
        }

        public int Id { get; }
        public string Name { get; }
    }
}