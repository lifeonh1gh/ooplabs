namespace Shops.Models
{
    public class SupplyProduct
    {
        public Product Product { get; }
        public double Price { get; }
        public int Amount { get; }

        public SupplyProduct(Product product, double price, int amount)
        {
            Product = product;
            Price = price;
            Amount = amount;
        }
    }
}