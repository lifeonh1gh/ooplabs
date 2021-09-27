using System.Collections.Generic;
using Isu.Models;
using Group = System.Text.RegularExpressions.Group;

namespace Isu.Interfaces
{
    public interface IIsu
    {
        Group AddGroup(string name);
        Student AddStudent(Group group, string name);

        Student GetStudent(int id);
        Student FindStudent(string name);
        List<Student> FindStudents(string groupName);

        // List<Student> FindStudents(CourseNumber courseNumber);
        Group FindGroup(string groupName);

        // List<Group> FindGroups(CourseNumber courseNumber);
        void ChangeStudentGroup(Student student, Group newGroup);
    }
}