using System;

namespace Shops.Models
{
    public class Person
    {
        public string Name { get; }
        public double Money { get; set; }

        public Person(string name, double money)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
            Money = money;
        }
    }
}