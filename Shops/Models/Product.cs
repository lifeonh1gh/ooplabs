using Shops.Tools;

namespace Shops.Models
{
    public class Product
    {
        public Product(int id, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ShopsException("Product name cannot be empty");
            }

            Id = id;
            Name = name;
        }

        public int Id { get; }
        public string Name { get; }
    }
}