using System;

namespace Shops.Models
{
    public class ShopProduct
    {
        public Shop Shop { get; set; }
        public Product Product { get; set; }
        public int Amount { get; set; }
        public double Price { get; set; }
    }
}