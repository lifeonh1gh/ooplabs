using System;
using System.Collections.Generic;
using IsuExtra.Models;

namespace IsuExtra.Interfaces
{
    public interface ICourseManager
    {
        Course CreateCourse(string name);
        CourseFlow GetStudent(int courseId);
        CourseFlow RegisterStudentToCourse(Course course, List<StudentEnrollment> studentsEnrollments);
        CourseFlow GetCourseFlow(int courseId);
        CourseFlow RemoveStudentOnCourse(int courseId, int studentId);
        List<StudentUnsigned> UnsignedStudentsOnCourse(List<StudentUnsigned> students, List<StudentEnrollment> studentEnrollments, string groupName);
    }
}