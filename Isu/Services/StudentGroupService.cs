using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Isu.Interfaces;
using Isu.Models;
using Isu.Tools;
using Group = Isu.Models.Group;

namespace Isu.Services
{
    public class StudentGroupService
    {
        public List<Student> Students { get; } = new List<Student>();
        public List<StudentGroup> StudentGroups { get; } = new List<StudentGroup>();

        public Student AddStudent(Group group, string name)
        {
            Students.Add(new Student() { StudentId = Students.Count, StudentName = name, GroupName = group });
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
            Console.Write($"Students under the group name - {groupName}: ");
            if (result == null)
            {
                return null;
            }

            return result;
        }

        public void ChangeStudentGroup(Student student, Group newGroup)
        {
        }
    }
}