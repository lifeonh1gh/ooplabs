using System;
using System.Collections.Generic;
using System.Linq;
using Isu.Services;
using Isu.Tools;

namespace Isu
{
    public class StudentGroup : IIsuService
    {
        private List<Group> Groups { get; } = new List<Group>();
        private List<Student> Students { get; } = new List<Student>();

        public int GenerateId()
        {
            Random generate = new Random();
            int rand = generate.Next(100000, 999999);
            return rand;
        }

        public Group AddGroup(string name)
        {
            if (name.StartsWith("M3"))
            {
                int.TryParse(name.Substring(3, 2), out var intId);
                Groups.Add(new Group(intId, name));
            }
            else
            {
                throw new IsuException();
            }

            return Groups.Last();
        }

        public Student AddStudent(Group group, string name)
        {
            int randId = GenerateId();
            const int count = 10;
            if (Students.Count < count)
            {
                Students.Add(new Student(randId, name, group));
            }
            else
            {
                throw new IsuException("Student in group limit reached");
            }

            return Students.Last();
        }

        public Student GetStudent(int id)
        {
            try
            {
                Student student = Students.FirstOrDefault(s => s.Id == id);
                return student;
            }
            catch (Exception e)
            {
                throw new IsuException(e.Message);
            }
        }

        public Student FindStudent(string name)
        {
            Student result = Students.Find(student => student.Name == name);
            Console.Write($"\nStudent under the name - {name}: ");
            return result;
        }

        public List<Student> FindStudents(string groupName)
        {
            List<Student> result = Students.FindAll(student => student.Group.Name == groupName);
            Console.WriteLine($"\nStudents under the group name - {groupName}: \n");
            return result;
        }

        public List<Student> FindStudents(int courseNumber)
        {
            IEnumerable<string> studentgroup = Students.Select(s => s.Group.Name).ToList();
            foreach (string s in studentgroup)
            {
                int.TryParse(s.Substring(2, 1), out var intValue);
                if (intValue == courseNumber)
                {
                    Console.WriteLine(courseNumber);
                }
            }

            return Enumerable.Empty<Student>() as List<Student>;
        }

        public Group FindGroup(string groupName)
        {
            Group result = Groups.Find(g => g.Name == groupName);
            Console.Write($"\nGroup under the name - {groupName}: ");
            return result;
        }

        public List<Group> FindGroups(int courseNumber)
        {
            IEnumerable<string> groups = Groups.Select(g => g.Name);
            foreach (var g in groups)
            {
                int.TryParse(g.Substring(2, 1), out var intValue);
                if (intValue == courseNumber)
                    Console.WriteLine(g);
            }

            return null;
        }

        public void ChangeStudentGroup(string name, Group newGroup)
        {
            try
            {
                int randId = GenerateId();
                Console.WriteLine($"\nStudents before changed group:\n");
                Students.RemoveAll((st) => st.Name == name);
                Students.Add(new Student(randId, name, newGroup));
                IEnumerable<Student> query = Students.Select(c => c);
                foreach (Student student in Students)
                {
                    Console.WriteLine($"\tId: {student.Id} \tName: {student.Name} \tNew group: {student.Group.Name}");
                }
            }
            catch (Exception e)
            {
                throw new IsuException($"Group change error for student", e);
            }
        }
    }
}