using System;
using System.Collections.Generic;

namespace Isu.Models
{
    public class Group
    {
        public Group(int id, string name)
        {
            GroupId = id;
            GroupName = name;
        }

        public int GroupId { get; set; }
        public string GroupName { get; set; }
    }
}