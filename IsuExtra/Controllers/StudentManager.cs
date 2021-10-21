using System;
using System.Collections.Generic;
using System.Linq;
using IsuExtra.Interfaces;
using IsuExtra.Models;
using IsuExtra.Tools;

namespace IsuExtra.Controllers
{
    public class StudentManager : IStudentManager
    {
        private List<Group> Groups { get; } = new List<Group>();
        private List<Student> Students { get; } = new List<Student>();

        public Group AddGroup(string name)
        {
            int.TryParse(name.Substring(3, 2), out var intId);
            Group temp = new Group(intId, name);
            Groups.Add(temp);
            return temp;
        }

        public Student AddStudent(Group group, string name)
        {
            const int count = 10;
            int id = 100000 + Students.Count;
            Student student = new Student(id, name, group);
            if (Students.Count < count)
            {
                Students.Add(student);
            }
            else
            {
                throw new IsuExtraException("Student in group limit reached");
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
                throw new IsuExtraException(e.Message);
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
            IEnumerable<string> groups = Groups.Where(g => g.IsCourseNumberEqualsTo(courseNumber)).Select(g => g.Name)
                .ToList();
            foreach (string s in groups)
            {
                return FindStudentsByGroup(s);
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
            IEnumerable<string> groups = Groups.Where(g => g.IsCourseNumberEqualsTo(courseNumber)).Select(g => g.Name);
            foreach (var g in groups)
            {
                return FindGroupsByName(g);
            }

            return Enumerable.Empty<Group>().ToList();
        }

        public void ChangeStudentGroup(string name, Group newGroup)
        {
            try
            {
                int oldId = Students[Index.Start].Id;
                Students.RemoveAll(st => st.Name == name);
                Students.Add(new Student(oldId, name, newGroup));
            }
            catch (Exception e)
            {
                throw new IsuExtraException("Group change error for student", e);
            }
        }
    }
}