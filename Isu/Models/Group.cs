using System;
using System.Collections.Generic;
using System.Linq;
using Isu.Interfaces;

namespace Isu.Models
{
    public class Group
    {
        public List<Group> Groups { get; } = new List<Group>();

        public int GroupId { get; set; }
        public string GroupName { get; set; }

        public Group AddGroup(string name)
        {
            Groups.Add(new Group() { GroupId = Groups.Count, GroupName = name });
            return Groups.Last();
        }

        public Group FindGroup(string groupName)
        {
            Group result = Groups.Find(sp => sp.GroupName == groupName);
            Console.Write($"Group under the name - {groupName}: ");
            if (result == null)
            {
                return null;
            }

            return result;
        }
    }
}