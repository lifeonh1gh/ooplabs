using System;
using Isu.Tools;

namespace Isu
{
    public class Group
    {
        public Group(int id, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new IsuException("Unable to create Group", new ArgumentNullException(nameof(name)));
            }

            Id = id;
            Name = name;
        }

        public int Id { get; }
        public string Name { get; }
    }
}