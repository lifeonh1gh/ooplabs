using System;
using Shops.Tools;

namespace Shops.Models
{
    public class SupplyProduct
    {
        public SupplyProduct(Product product, double price, int amount)
        {
            Product = product;
            if (Product == null)
            {
                throw new NullReferenceException(nameof(Product.Name));
            }

            Price = price;
            Amount = amount;
        }

        public Product Product { get; }
        public double Price { get; }
        public int Amount { get; }
    }
}