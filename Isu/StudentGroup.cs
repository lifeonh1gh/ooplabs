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
            return rand + 1;
        }

        public Group AddGroup(string name)
        {
            if (name.StartsWith("M3"))
            {
                int.TryParse(name.Substring(3, 2), out var intId);
                Group temp = new Group(intId, name);
                Groups.Add(temp);
                return temp;
            }
            else
            {
                throw new IsuException();
            }
        }

        public Student AddStudent(Group group, string name)
        {
            int randId = GenerateId();
            const int count = 10;
            Student student = new Student(randId, name, group);
            if (Students.Count < count)
            {
                Students.Add(student);
            }
            else
            {
                throw new IsuException("Student in group limit reached");
            }

            return student;
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
            return result;
        }

        public List<Student> FindStudentsByGroup(string groupName)
        {
            List<Student> result = Students.FindAll(student => student.Group.Name == groupName);
            return result;
        }

        public List<Student> FindStudentsByCourse(int courseNumber)
        {
            IEnumerable<string> studentgroup = Students.Select(s => s.Group.Name).ToList();
            foreach (string s in studentgroup)
            {
                int.TryParse(s.Substring(2, 1), out var intValue);
                if (intValue == courseNumber)
                {
                    return FindStudentsByGroup(s);
                }
            }

            return Enumerable.Empty<Student>().ToList();
        }

        public Group FindGroup(string groupName)
        {
            Group result = Groups.Find(g => g.Name == groupName);
            return result;
        }

        public List<Group> FindGroupsByName(string groupName)
        {
            List<Group> result = Groups.FindAll(group => group.Name == groupName);
            return result;
        }

        public List<Group> FindGroups(int courseNumber)
        {
            IEnumerable<string> groups = Groups.Select(g => g.Name);
            foreach (var g in groups)
            {
                int.TryParse(g.Substring(2, 1), out var intValue);
                if (intValue == courseNumber)
                    return FindGroupsByName(g);
            }

            return Enumerable.Empty<Group>().ToList();
        }

        public void ChangeStudentGroup(string name, Group newGroup)
        {
            Console.WriteLine("\nStudents before changed group:");
            try
            {
                int oldId = Students[Index.Start].Id;
                Students.RemoveAll(st => st.Name == name);
                Students.Add(new Student(oldId, name, newGroup));
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