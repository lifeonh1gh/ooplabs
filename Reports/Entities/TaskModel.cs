using System;

namespace Reports.Entities
{
    public class TaskModel
    {
        public Guid Id { get; set;}
        public TaskState State { get; set; }
        public string Name { get; set;}
        public string Description { get; set;}
        public Employee AssignedEmployee { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ClosingDate { get; set; }
    }
}