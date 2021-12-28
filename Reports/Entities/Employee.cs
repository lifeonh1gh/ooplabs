using System;

namespace Reports.Entities
{
    public class Employee
    {
        public Guid Id { get; set; }
        public Role Role { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public Guid MentorId { get; set; }
        public string MentorName { get; set; }
    }
}