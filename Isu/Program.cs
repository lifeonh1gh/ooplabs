using System;
using System.Collections.Generic;
using System.Text.Json;
using Isu.Interfaces;
using Isu.Models;
using Isu.Services;

namespace Isu
{
    internal class Program
    {
        private static void PrintJson(object obj)
        {
            Console.WriteLine(JsonSerializer.Serialize(obj, new JsonSerializerOptions() { WriteIndented = true }));
        }

        private static void Main()
        {
            var temp = new Group();
            Group group1 = temp.AddGroup("M3312");
            Group group2 = temp.AddGroup("M3313");

            PrintJson(group1);
            PrintJson(group2);
            var control = new StudentGroupService();
            Student student1 = control.AddStudent(group1, "radik");
            Student student2 = control.AddStudent(group2, "ivan");
            Student student3 = control.AddStudent(group1, "roman");
            PrintJson(student1);
            PrintJson(student2);
            PrintJson(student3);

            PrintJson(control.GetStudent(2).StudentName);
            PrintJson(control.FindStudent("radik").StudentName);
            PrintJson(temp.FindGroup("M3312").GroupName);
            PrintJson(control.FindStudents("M331"));
        }
    }
}