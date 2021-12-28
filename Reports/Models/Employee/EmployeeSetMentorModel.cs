using System;
using System.ComponentModel.DataAnnotations;

namespace Reports.Models.Employee
{
    public class EmployeeSetMentorModel
    {
        [Required]
        public Guid MentorId { get; set; }
        [Required]
        public string MentorName { get; set; }
    }
}