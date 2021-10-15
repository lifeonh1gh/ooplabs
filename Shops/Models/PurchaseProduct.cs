namespace Shops.Models
{
    public class PurchaseProduct
    {
        public Product Product { get; }
        public int Amount { get; }

        public PurchaseProduct(Product product, int amount)
        {
            Product = product;
            Amount = amount;
        }
    }
}