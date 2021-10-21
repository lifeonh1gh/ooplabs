using System;
using System.Collections.Generic;
using IsuExtra.Controllers;
using IsuExtra.Models;
using static System.Console;

namespace IsuExtra
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var studentManager = new StudentManager();

            WriteLine("\nAdded groups:\n");
            Group m1 = studentManager.AddGroup("M1211");
            Group с2 = studentManager.AddGroup("C2312");
            WriteLine($"\tId: {m1.Id} \tName: {m1.Name}");
            WriteLine($"\tId: {с2.Id} \tName: {с2.Name}");

            WriteLine("\nAdded students:\n");
            Student student1 = studentManager.AddStudent(m1, "rodion");
            Student student2 = studentManager.AddStudent(m1, "ivan");
            Student student3 = studentManager.AddStudent(m1, "roman");
            Student student4 = studentManager.AddStudent(с2, "kirill");
            Student student5 = studentManager.AddStudent(с2, "daniil");
            Student student6 = studentManager.AddStudent(с2, "alex");
            WriteLine($"\tId: {student1.Id} \tName: {student1.Name} \tGroup: {student1.Group.Name}");
            WriteLine($"\tId: {student2.Id} \tName: {student2.Name} \tGroup: {student2.Group.Name}");
            WriteLine($"\tId: {student3.Id} \tName: {student3.Name} \tGroup: {student3.Group.Name}");

            var courseManager = new CourseManager(studentManager);
            WriteLine("\nAdded courses:\n");
            Course ct = courseManager.CreateCourse("Computer technology");
            WriteLine($"\tCourseId: {ct.Id}, \tCourseName: {ct.Name}");
            Course mg = courseManager.CreateCourse("Mathematical technology");
            WriteLine($"\tCourseId: {mg.Id}, \tCourseName: {mg.Name}");

            WriteLine("\nAdded students to course:\n");
            var studentEnrollment = new List<StudentEnrollment>
            {
                new StudentEnrollment(student2),
                new StudentEnrollment(student3),
            };
            courseManager.RegisterStudentToCourse(ct, studentEnrollment);
            foreach (var se in studentEnrollment)
            {
                WriteLine(
                    $"\tCourseId: {ct.Id}, \tCourse: {ct.Name}, \tStudentId: {se.Student.Id}, \tStudentName: {se.Student.Name}, \tStudentGroup: {se.Student.Group.Name}");
            }

            WriteLine("\nAll unsigned students on course:\n");

            var studentUnsigned = new List<StudentUnsigned>
            {
                new StudentUnsigned(student1),
                new StudentUnsigned(student2),
                new StudentUnsigned(student3),
                new StudentUnsigned(student4),
                new StudentUnsigned(student5),
                new StudentUnsigned(student6),
            };
            courseManager.UnsignedStudentsOnCourse(studentUnsigned, studentEnrollment, "M1211");
        }
    }
}