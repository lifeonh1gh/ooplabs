using System;
using System.Collections.Generic;
using System.Linq;
using IsuExtra.Interfaces;
using IsuExtra.Models;
using IsuExtra.Tools;

namespace IsuExtra.Controllers
{
    public class CourseManager : ICourseManager
    {
        public CourseManager(StudentManager studentManager)
        {
            StudentManager = studentManager;
        }

        private List<Course> Courses { get; } = new List<Course>();
        private List<CourseFlow> CoursesFlows { get; } = new List<CourseFlow>();
        private StudentManager StudentManager { get; }

        public Course CreateCourse(string name)
        {
            Course course = new Course(Courses.Count, name);
            Courses.Add(course);
            return course;
        }

        public CourseFlow GetStudents(int courseId)
        {
            try
            {
                CourseFlow students = CoursesFlows.First(s => s.Course.Id == courseId);
                return students;
            }
            catch (Exception e)
            {
                throw new IsuExtraException(e.Message);
            }
        }

        public CourseFlow RegisterStudentToCourse(Course course, List<StudentEnrollment> studentsEnrollments)
        {
            var nameCourse = Courses.ElementAt(course.Id).Name.Substring(0, 1);
            const int maxCount = 10;
            foreach (var st in studentsEnrollments.Select(student => new CourseFlow()
                { Course = course, Student = student.Student }))
            {
                if (nameCourse != st.Student.Group.Name.Substring(0, 1) && CoursesFlows.Count < maxCount)
                {
                    CoursesFlows.Add(st);
                }
                else
                {
                    throw new IsuExtraException(
                        "Students in course limit or the student cannot enroll in their group's course");
                }
            }

            return GetStudents(course.Id);
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

        public int GetCourseFlowCount(int courseId)
        {
            var flow = CoursesFlows.Where(c => c.Course.Id == courseId).ToList();
            return flow.Count;
        }

        public CourseFlow RemoveStudentOnCourse(int courseId, int studentId)
        {
            try
            {
                var flow = CoursesFlows.Where(c => c.Course.Id == courseId && c.Student.Id == studentId).ToList();
                CoursesFlows.RemoveAll(c => c.Student.Id == studentId);
                return GetStudents(courseId);
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