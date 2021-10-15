using System;

namespace Shops.Models
{
    public class ShopProduct
    {
        public ShopProduct(Shop shop, Product product, int amount, double price)
        {
            Shop = shop ?? throw new NullReferenceException(nameof(Shop.Name));
            Product = product ?? throw new NullReferenceException(nameof(Product.Name));
            Amount = amount;
            Price = price;
        }

        public Shop Shop { get; }
        public Product Product { get; }
        public int Amount { get; set; }
        public double Price { get; }
    }
}