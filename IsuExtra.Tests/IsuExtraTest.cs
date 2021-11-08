using System.Collections.Generic;
using IsuExtra.Services;
using IsuExtra.Models;
using IsuExtra.Tools;
using NUnit.Framework;

namespace IsuExtra.Tests
{
    public class IsuExtraTest
    {
        private StudentManager _studentManager;
        private CourseManager _courseManager;
        private Group _m1;
        private Group _c2;
        private Student _student1;
        private Student _student2;
        private Student _student3;
        private Student _student4;
        private Student _student5;
        private Student _student6;
        private Student _student7;
        private Student _student8;
        private Student _student9;
        private Student _student10;
        private Student _student11;
        private Course _ct;
        private Course _mt;
        private List<StudentEnrollment> _studentEnrollments;
        private List<StudentUnsigned> _studentUnsigneds;

        [SetUp]
        public void Setup()
        {
            _studentManager = new StudentManager();
            _courseManager = new CourseManager();
            _m1 = _studentManager.AddGroup("M1211");
            _c2 = _studentManager.AddGroup("C2312");
            _ct = _courseManager.CreateCourse("Computer technology");
            _mt = _courseManager.CreateCourse("Mathematical technology");
        }

        [Test]
        public void RegisterStudentToCourse_StudentsEnrolled()
        {
            _student1 = _studentManager.AddStudent(_m1, "rodion");
            _student2 = _studentManager.AddStudent(_m1, "ivan");
            _studentEnrollments = new List<StudentEnrollment>
            {
                new StudentEnrollment(_student1),
                new StudentEnrollment(_student2),
            };
            var expected = _courseManager.RegisterStudentToCourse(_ct, _studentEnrollments);
            var actual = _courseManager.GetCourseFlow(_ct.Id);
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void RegisterStudentToCoursePresentHisGroup_ThrowException()
        {
            _student11 = _studentManager.AddStudent(_m1, "sergey");
            _studentEnrollments = new List<StudentEnrollment>
            {
                new StudentEnrollment(_student11),
            };
            Assert.Catch<IsuExtraException>(() => _courseManager.RegisterStudentToCourse(_mt, _studentEnrollments));
        }

        [Test]
        public void ReachMaxStudentInCourse_ThrowException()
        {
            _student1 = _studentManager.AddStudent(_m1, "rodion");
            _student2 = _studentManager.AddStudent(_m1, "ivan");
            _student3 = _studentManager.AddStudent(_m1, "roman");
            _student4 = _studentManager.AddStudent(_m1, "kirill");
            _student5 = _studentManager.AddStudent(_m1, "daniil");
            _student6 = _studentManager.AddStudent(_m1, "alex");
            _student7 = _studentManager.AddStudent(_m1, "alex2");
            _student8 = _studentManager.AddStudent(_m1, "alex3");
            _student9 = _studentManager.AddStudent(_m1, "alex4");
            _student10 = _studentManager.AddStudent(_m1, "alex5");
            _studentEnrollments = new List<StudentEnrollment>
            {
                new StudentEnrollment(_student1),
                new StudentEnrollment(_student2),
                new StudentEnrollment(_student3),
                new StudentEnrollment(_student4),
                new StudentEnrollment(_student5),
                new StudentEnrollment(_student6),
                new StudentEnrollment(_student7),
                new StudentEnrollment(_student8),
                new StudentEnrollment(_student9),
                new StudentEnrollment(_student10),
            };
            _courseManager.RegisterStudentToCourse(_ct, _studentEnrollments);
            Assert.Catch<IsuExtraException>(() => _courseManager.RegisterStudentToCourse(_ct, _studentEnrollments));
        }

        [Test]
        public void RemoveStudentFromCourse_StudentsCountChanged()
        {
            _student1 = _studentManager.AddStudent(_m1, "rodion");
            _student2 = _studentManager.AddStudent(_m1, "ivan");
            _student3 = _studentManager.AddStudent(_m1, "roman");
            _studentEnrollments = new List<StudentEnrollment>
            {
                new StudentEnrollment(_student1),
                new StudentEnrollment(_student2),
                new StudentEnrollment(_student3),
            };
            _courseManager.RegisterStudentToCourse(_ct, _studentEnrollments);
            var expected = _courseManager.RemoveStudentOnCourse(0, 100000);
            var actual = _courseManager.GetStudent(0);
            Assert.AreEqual(expected, actual);
        }
    }
}