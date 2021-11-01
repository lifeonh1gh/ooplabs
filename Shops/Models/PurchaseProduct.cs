using System;
using Shops.Tools;

namespace Shops.Models
{
    public class PurchaseProduct
    {
        public PurchaseProduct(Product product, int amount)
        {
            Product = product;
            if (Product == null)
            {
                throw new NullReferenceException(nameof(Product.Name));
            }

            Amount = amount;
        }

        public Product Product { get; }
        public int Amount { get; }
    }
}