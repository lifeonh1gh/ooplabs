using System;
using System.Collections.Generic;
using System.Text.Json;
using Isu.Models;
using Isu.Services;

namespace Isu
{
    internal class Program
    {
        private static void PrintJson(object obj)
        {
            Console.WriteLine(JsonSerializer.Serialize(obj, new JsonSerializerOptions() {WriteIndented = true}));
        }

        private static void Main()
        {
            var temp = new StudentGroupService();
            Group group1 = temp.AddGroup("MZ312");
            Group group2 = temp.AddGroup("MZ313");

            Student student1 = temp.AddStudent(group1, "radik");
            Student student2 = temp.AddStudent(group2, "ivan");
            Student student3 = temp.AddStudent(group2, "roman");

            /*PrintJson(student1);
            PrintJson(student2);
            PrintJson(student3);*/

            /*PrintJson(temp.GetStudent(2).StudentName);
            PrintJson(temp.FindStudent("radik").StudentName);
            PrintJson(temp.FindGroup("MZ312").GroupName);
            PrintJson(temp.FindStudents("MZ312"));
            PrintJson(temp.FindGroups("MZ"));*/
            /*temp.ChangeStudentGroup("radik", "3123");*/
        }
    }
}