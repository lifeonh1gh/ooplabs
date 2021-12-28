using System.ComponentModel.DataAnnotations;
using Reports.Entities;

namespace Reports.Models.Employee
{
    public class EmployeeCreateModel
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        [EnumDataType(typeof(Role))]
        public string Role { get; set; }
        
        [Required]
        public string Email { get; set; }
        
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
        
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}