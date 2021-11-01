using System;
using IsuExtra.Tools;

namespace IsuExtra.Models
{
    public class Student
    {
        public Student(int id, string name, Group group)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new IsuExtraException("Student name cannot be empty");
            }

            Id = id;
            Name = name;
            Group = group;
            if (Group == null)
            {
                throw new NullReferenceException(nameof(Group.Name));
            }
        }

        public int Id { get; }
        public string Name { get; set; }
        public Group Group { get; }
    }
}