using System.Collections.Generic;

namespace Isu.Services
{
    public interface IIsuService
    {
        Group AddGroup(string name);
        Student AddStudent(Group group, string name);
        Student GetStudent(int id);
        Student FindStudent(string name);
        List<Student> FindStudentsByGroup(string groupName);
        List<Student> FindStudentsByCourse(int courseNumber);
        Group FindGroup(string groupName);
        List<Group> FindGroupsByName(string groupName);
        List<Group> FindGroups(int courseNumber);
        void ChangeStudentGroup(string name, Group newGroup);
    }
}