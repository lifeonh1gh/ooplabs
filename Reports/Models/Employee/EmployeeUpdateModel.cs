using System.ComponentModel.DataAnnotations;
using Reports.Entities;

namespace Reports.Models.Employee
{
    public class EmployeeUpdateModel
    {
        public string Name { get; set; }

        [EnumDataType(typeof(Role))]
        public string Role { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        
        private string _password;
        [MinLength(6)]
        public string Password
        {
            get => _password;
            set => _password = replaceEmptyWithNull(value);
        }

        private string _confirmPassword;
        [Compare("Password")]
        public string ConfirmPassword 
        {
            get => _confirmPassword;
            set => _confirmPassword = replaceEmptyWithNull(value);
        }

        private string replaceEmptyWithNull(string value)
        {
            return string.IsNullOrEmpty(value) ? null : value;
        }
    }
}