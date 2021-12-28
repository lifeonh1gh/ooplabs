using System;

namespace Reports.Entities
{
    public class Comment
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public TaskModel Task { get; set; }
        public Employee SenderEmployee { get; set; }
    }
}