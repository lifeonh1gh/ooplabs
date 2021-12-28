﻿using System;
using System.ComponentModel.DataAnnotations;
using Reports.Entities;

namespace Reports.Models.Task
{
    public class TaskCreateModel
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Description { get; set; }
        
        [Required]
        [EnumDataType(typeof(TaskState))]
        public string State { get; set; }

        [Required]
        public DateTime ClosingDate { get; set; }
    }
}