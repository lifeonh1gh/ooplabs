using Isu.Services;
using Isu.Tools;
using NUnit.Framework;

namespace Isu.Tests
{
    public class Tests
    {
        private StudentGroup _temp;
        private Group _group1;
        private Group _group2;

        [SetUp]
        public void Setup()
        {
            _temp = new StudentGroup();
            _group1 = _temp.AddGroup("M3302");
            _group2 = _temp.AddGroup("M3403");
        }

        [Test]
        public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
        {
            Student expected = _temp.AddStudent(_group1, "radik");
            Student actual = _temp.FindStudent("radik");
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ReachMaxStudentPerGroup_ThrowException()
        {
            const int count = 10;
            for (int i = 0; i < count; i++)
            {
                _temp.AddStudent(_group1, "radik");
            }
            Assert.Catch<IsuException>(() => _temp.AddStudent(_group1, "radik"));
        }

        [Test]
        public void CreateGroupWithInvalidName_ThrowException()
        {
            Assert.Catch<IsuException>(() => _temp.AddGroup("M4201"));
        }

        [Test]
        public void TransferStudentToAnotherGroup_GroupChanged()
        {
            _temp.AddStudent(_group1, "radik");
            _temp.ChangeStudentGroup("radik", _group2);
            Assert.AreEqual(_group2, _group2);
        }
    }
}