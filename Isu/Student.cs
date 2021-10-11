using System;

namespace Isu
{
    public class Student
    {
        public Student(int id, string name, Group group)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Id = id;
            Name = name;
            Group = group ?? throw new NullReferenceException(nameof(Group.Name));
        }

        public int Id { get; }
        public string Name { get; }
        public Group Group { get; }
    }
}