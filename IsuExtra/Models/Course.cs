using System;
using IsuExtra.Tools;

namespace IsuExtra.Models
{
    public class Course
    {
        public Course(int id, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new IsuExtraException("Course name cannot be empty");
            }

            Id = id;
            Name = name;
        }

        public int Id { get; }
        public string Name { get; }
    }
}