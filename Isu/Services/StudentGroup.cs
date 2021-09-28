using Isu.Models;

namespace Isu.Services
{
    public class StudentGroup
    {
        public StudentGroup(int id, Group newGroup, Student student)
        {
            StudentGroupId = id;
            Group = newGroup;
            Student = student;
        }

        public int StudentGroupId { get; set; }
        public Group Group { get; }
        public Student Student { get; }
    }
}