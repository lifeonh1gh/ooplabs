using Isu.Services;
using Isu.Tools;
using NUnit.Framework;

namespace Isu.Tests
{
    public class Tests
    {
        private IIsuService _isuService;

        [SetUp]
        public void Setup()
        {
            //TODO: implement
            _isuService = null;
        }

        [Test]
        public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
        {
            var temp = new StudentGroup();
            Group group1 = temp.AddGroup("M3302");
            Student student1 = temp.AddStudent(group1, "radik");
            Student actual = temp.FindStudent("radik");
            Assert.AreEqual(student1, actual);
        }

        [Test]
        public void ReachMaxStudentPerGroup_ThrowException()
        {
            const int count = 10;
            var temp = new StudentGroup();
            Group group1 = temp.AddGroup("M3201");
            for (int i = 0; i < count; i++)
            {
                temp.AddStudent(group1, "radik");
            }
            Assert.Catch<IsuException>(() => temp.AddStudent(group1, "radik"));
        }

        [Test]
        public void CreateGroupWithInvalidName_ThrowException()
        {
            var temp = new StudentGroup();
            Assert.Catch<IsuException>(() => temp.AddGroup("M4201"));
        }

        [Test]
        public void TransferStudentToAnotherGroup_GroupChanged()
        {
            var temp = new StudentGroup();
            Group group1 = temp.AddGroup("M3302");
            temp.AddStudent(group1, "radik");
            Group group2 = temp.AddGroup("M3403");
            temp.ChangeStudentGroup("radik", group2);
            Assert.AreEqual(group2, group2);
        }
    }
}