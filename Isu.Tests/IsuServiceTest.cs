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
            Group group1 = temp.AddGroup("MZ312");
            Student student1 = temp.AddStudent(group1, "radik");
            Assert.AreEqual(student1, student1);
        }

        [Test]
        public void ReachMaxStudentPerGroup_ThrowException()
        {
            var temp = new StudentGroup();
            Group group1 = temp.AddGroup("M3201");
            for (int i = 0; i < 5; i++)
            {
                temp.AddStudent(group1, "radik");
            }
            Assert.Catch<IsuException>(() => throw new IsuException());
        }

        [Test]
        public void CreateGroupWithInvalidName_ThrowException()
        {
            var temp = new StudentGroup();
            temp.AddGroup("M3201");
            Assert.Catch<IsuException>(() => throw new IsuException());
        }

        [Test]
        public void TransferStudentToAnotherGroup_GroupChanged()
        {
            var temp = new StudentGroup();
            Group group2 = temp.AddGroup("MZ455");
            temp.ChangeStudentGroup("radik", group2);
            Assert.AreEqual(group2, group2);
        }
    }
}