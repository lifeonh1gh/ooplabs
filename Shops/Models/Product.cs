using System;

namespace Shops.Models
{
    public class Product
    {
        public Product(int id, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Id = id;
            Name = name;
        }

        public int Id { get; }
        public string Name { get; }
    }
}