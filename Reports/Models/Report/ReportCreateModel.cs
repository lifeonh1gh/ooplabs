﻿using System.ComponentModel.DataAnnotations;
using Reports.Entities;

namespace Reports.Models.Report
{
    public class ReportCreateModel
    {
        [Required]
        public string Description { get; set; }
        
        [Required]
        public ReportState State { get; set; }
    }
}