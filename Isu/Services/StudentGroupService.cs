using System;
using System.Collections.Generic;
using System.Linq;
using Isu.Models;
using Isu.Tools;

namespace Isu.Services
{
    public class StudentGroupService : IIsuService
    {
        public List<Group> Groups { get; } = new List<Group>();
        public List<Student> Students { get; } = new List<Student>();
        public List<StudentGroup> StudentGroups { get; } = new List<StudentGroup>();
        public Group AddGroup(string name)
        {
            Groups.Add(new Group(Groups.Count, name));
            return Groups.Last();
        }

        public Student AddStudent(Group group, string name)
        {
            Students.Add(new Student(Students.Count, name, group));
            return Students.Last();
        }

        public Student GetStudent(int id)
        {
            IsuException ex = new IsuException($"No such Student under the index - {id}");
            try
            {
                Student result = Students.Find(sp => sp.StudentId == id);
                Console.Write($"Student under the index - {id}: ");
                return result;
            }
            catch (IsuException e)
            {
                Console.WriteLine(e.Message);
            }

            throw ex;
        }

        public Student FindStudent(string name)
        {
            Student result = Students.Find(sp => sp.StudentName == name);
            Console.Write($"Student under the name - {name}: ");
            if (result == null)
            {
                return null;
            }

            return result;
        }

        public List<Student> FindStudents(string groupName)
        {
            List<Student> result = Students.FindAll(sp => sp.GroupName.GroupName == groupName);
            Console.WriteLine($"Students under the group name - {groupName}: ");
            return result;
        }

        public List<Student> FindStudents(CourseNumber courseNumber)
        {
            throw new NotImplementedException();
        }

        public Group FindGroup(string groupName)
        {
            Group result = Groups.Find(sp => sp.GroupName == groupName);
            Console.Write($"Group under the name - {groupName}: ");
            if (result == null)
            {
                return null;
            }

            return result;
        }

        public List<Group> FindGroups(CourseNumber courseNumber)
        {
            throw new NotImplementedException();
        }

        public List<Group> FindGroups(string groupName)
        {
            List<Group> result = Groups.FindAll(sp => sp.GroupName.Contains(groupName));
            Console.WriteLine($"All find groups:");
            return result;
        }

        public void ChangeStudentGroup(string student, string newGroup)
        {
            IEnumerable<Student> result = Students.Select(groupName => Students.SingleOrDefault(p => p.GroupName == groupName.GroupName) ?? groupName);
        }
    }
}