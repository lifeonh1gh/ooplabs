namespace Isu.Models
{
    public class Student
    {
        public Student(int id, string name, Group group)
        {
            StudentId = id;
            StudentName = name;
            GroupName = group;
        }

        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public Group GroupName { get; set; }
    }
}