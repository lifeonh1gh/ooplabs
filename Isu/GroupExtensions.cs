namespace Isu
{
    public static class GroupExtensions
    {
        public static bool IsCourseNumberEqualsTo(this Group group, int courseNumber)
        {
            int.TryParse(group.Name.Substring(2, 1), out var intValue);
            if (intValue == courseNumber)
                return true;
            return false;
        }
    }
}