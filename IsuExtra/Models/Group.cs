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
                throw new IsuExtraException("Unable to create Group", new ArgumentNullException(nameof(name)));
            }

            Id = id;
            Name = name;
        }

        public int Id { get; }
        public string Name { get; }
    }
}