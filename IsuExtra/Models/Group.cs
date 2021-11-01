using System;
using IsuExtra.Tools;

namespace IsuExtra.Models
{
    public class Group
    {
        public Group(int id, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new IsuExtraException("Group name cannot be empty");
            }

            Id = id;
            Name = name;
        }

        public int Id { get; }
        public string Name { get; }
    }
}