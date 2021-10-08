using System;
using System.Collections.Generic;
using System.Linq;
using Isu.Tools;

namespace Isu
{
    public class StudentGroup
    {
        private List<Group> Groups { get; } = new List<Group>();
        private List<Student> Students { get; } = new List<Student>();

        public Group AddGroup(string name)
        {
            if (Groups.Count < 3)
            {
                if (name.StartsWith("M3"))
                {
                    Groups.Add(new Group(Groups.Count, name));
                }
                else
                {
                    Console.WriteLine("Invalid name of group!");
                }
            }
            else
            {
                throw new IsuException();
            }

            return Groups.Last();
        }

        public Student AddStudent(Group group, string name)
        {
            if (Students.Count < 10)
            {
                Students.Add(new Student(Students.Count, name, group));
            }
            else
            {
                throw new Exception();
            }

            return Students.Last();
        }

        public Student GetStudent(int id)
        {
            Student student = (from s in Students where s.Id == id select s).First();
            if (student != null)
            {
                Console.Write($"\nStudent under the index - {id}: ");
                return student;
            }
            else
            {
                throw new IsuException();
            }
        }

        public Student FindStudent(string name)
        {
            Student result = Students.Find(student => student.Name == name);
            Console.Write($"\nStudent under the name - {name}: ");
            if (result == null)
            {
                return null;
            }

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

            return null;
        }

        public Group FindGroup(string groupName)
        {
            Group result = Groups.Find(g => g.Name == groupName);
            Console.Write($"\nGroup under the name - {groupName}: ");
            if (result == null)
            {
                return null;
            }

            return result;
        }

        public List<Group> FindGroups(int courseNumber)
        {
            IEnumerable<string> groups = Groups.Select(g => g.Name);
            foreach (var g in groups)
            {
                /*string cn = g.Substring(2, 1);*/
                int.TryParse(g.Substring(2, 1), out var intValue);
                if (intValue == courseNumber)
                    Console.WriteLine(g);
                /*Console.WriteLine(cn);*/
            }

            return null;
        }

        public void ChangeStudentGroup(string name, Group newGroup)
        {
            var ex = new IsuException($"Group change error for student");
            try
            {
                Console.WriteLine($"\nStudents before changed group:\n");
                Students.RemoveAll((st) => st.Name == name);
                Students.Add(new Student(Students.Count + 1, name, newGroup));
                IEnumerable<Student> query = Students.Select(c => c);
                foreach (Student student in Students)
                {
                    Console.WriteLine($"\tId: {student.Id} \tName: {student.Name} \tNew group: {student.Group.Name}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw ex;
            }
        }
    }
}