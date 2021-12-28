using System.ComponentModel.DataAnnotations;
using Reports.Entities;

namespace Reports.Models.Report
{
    public class TaskInsertModel
    {
        [Required]
        public string TaskName { get; set; }
    }
}