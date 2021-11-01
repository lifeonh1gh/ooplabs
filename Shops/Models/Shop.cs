using System;
using Shops.Tools;

namespace Shops.Models
{
    public class Shop
    {
        public Shop(int id, string name, string address)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ShopsException("Shop name cannot be empty");
            }

            if (string.IsNullOrWhiteSpace(address))
            {
                throw new ShopsException("Shop address cannot be empty");
            }

            Id = id;
            Name = name;
            Address = address;
        }

        public int Id { get; }
        public string Name { get; }
        public string Address { get; }
    }
}