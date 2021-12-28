using System;

namespace Reports.Entities
{
    public class WeeklyReport
    {
        public Guid Id { get; set; }
        public Report Report { get; set; }
        public TaskModel Task { get; set; }
    }
}