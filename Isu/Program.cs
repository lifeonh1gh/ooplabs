using System;
using System.Collections.Generic;
using static System.Console;

namespace Isu
{
    internal class Program
    {
        private static void Main()
        {
            var temp = new StudentGroup();

            WriteLine("\nAdded groups:\n");
            Group group1 = temp.AddGroup("M3201");
            Group group2 = temp.AddGroup("M3302");
            Group group3 = temp.AddGroup("M3403");
            WriteLine($"\tId: {group1.Id} \tName: {group1.Name}");
            WriteLine($"\tId: {group2.Id} \tName: {group2.Name}");
            WriteLine($"\tId: {group3.Id} \tName: {group3.Name}");

            WriteLine("\nAdded students:\n");
            Student student1 = temp.AddStudent(group1, "radik");
            Student student2 = temp.AddStudent(group2, "ivan");
            Student student3 = temp.AddStudent(group2, "roman");
            WriteLine($"\tID: {student1.Id} \tNAME: {student1.Name} \tGROUP: {student1.Group.Name}");
            WriteLine($"\tID: {student2.Id} \tNAME: {student2.Name} \tGROUP: {student2.Group.Name}");
            WriteLine($"\tID: {student3.Id} \tNAME: {student3.Name} \tGROUP: {student3.Group.Name}");

            temp.ChangeStudentGroup("radik", group3);

            WriteLine($"\nAll groups found under the course number:");
            List<Group> groups = temp.FindGroups(2);
            foreach (var gr in groups)
            {
                WriteLine($"\tId: {gr.Id} \tName: {gr.Name}");
            }

            WriteLine($"\nAll students found under the course number:");
            List<Student> students = temp.FindStudentsByCourse(3);
            foreach (var st in students)
            {
                WriteLine($"\tId: {st.Id} \tName: {st.Name} \tGroup: {st.Group.Name}");
            }
        }
    }
}