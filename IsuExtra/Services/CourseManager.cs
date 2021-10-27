using System;
using System.Collections.Generic;
using System.Linq;
using IsuExtra.Interfaces;
using IsuExtra.Models;
using IsuExtra.Tools;

namespace IsuExtra.Services
{
    public class CourseManager : ICourseManager
    {
        private List<Course> Courses { get; } = new List<Course>();
        private List<CourseFlow> CoursesFlows { get; } = new List<CourseFlow>();

        public Course CreateCourse(string name)
        {
            var course = new Course(Courses.Count, name);
            Courses.Add(course);
            return course;
        }

        public CourseFlow GetStudent(int courseId)
        {
            var student = CoursesFlows.FirstOrDefault(s => s.Course.Id == courseId) ??
                           throw new IsuExtraException("Student not found");
            return student;
        }

        public CourseFlow RegisterStudentToCourse(Course course, List<StudentEnrollment> studentsEnrollments)
        {
            var nameCourse = Courses.ElementAt(course.Id).Name.Substring(0, 1);
            const int maxCount = 10;
            foreach (var st in studentsEnrollments.Select(student => new CourseFlow()
                { Course = course, Student = student.Student }))
            {
                if (nameCourse == st.Student.Group.Name.Substring(0, 1))
                {
                    throw new IsuExtraException(
                        "Student cannot enroll in their group's course");
                }

                if (CoursesFlows.Count > maxCount)
                {
                    throw new IsuExtraException(
                        "Students in course limit");
                }

                CoursesFlows.Add(st);
            }

            return GetStudent(course.Id);
        }

        public CourseFlow GetCourseFlow(int courseId)
        {
            var result = CoursesFlows.Find(cf => cf.Course.Id == courseId);
            if (result == null)
            {
                throw new IsuExtraException("No such students in course");
            }

            return result;
        }

        public CourseFlow RemoveStudentOnCourse(int courseId, int studentId)
        {
            try
            {
                var flow = CoursesFlows.Where(c => c.Course.Id == courseId && c.Student.Id == studentId).ToList();
                CoursesFlows.RemoveAll(c => c.Student.Id == studentId);
                return GetStudent(courseId);
            }
            catch (Exception e)
            {
                throw new IsuExtraException(e.Message);
            }
        }

        public List<StudentUnsigned> UnsignedStudentsOnCourse(List<StudentUnsigned> students, List<StudentEnrollment> studentEnrollments, string groupName)
        {
            var list = new List<StudentUnsigned>();

            foreach (var res in students.Where(s => s.Student.Group.Name == groupName))
            {
                var student = res.Student.Name != null
                    ? studentEnrollments.FirstOrDefault(s => s.Student.Name == res.Student.Name)
                    : studentEnrollments.FirstOrDefault(s => s.Student.Id == res.Student.Id);
                if (student == null)
                {
                    list.Add(res);
                }
            }

            return list;
        }
    }
}