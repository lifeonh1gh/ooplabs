using System;

namespace Reports.Entities
{
    public class Report
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public ReportState State { get; set; }
        public Employee AssignedEmployee { get; set; }
    }
}