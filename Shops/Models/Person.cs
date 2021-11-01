using Shops.Tools;

namespace Shops.Models
{
    public class Person
    {
        public Person(string name, double money)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ShopsException("Person name cannot be empty");
            }

            Name = name;
            Money = money;
        }

        public string Name { get; }
        public double Money { get; set; }
    }
}