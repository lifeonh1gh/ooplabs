namespace Shops.Models
{
    public class PurchaseProduct
    {
        public PurchaseProduct(Product product, int amount)
        {
            Product = product;
            Amount = amount;
        }

        public Product Product { get; }
        public int Amount { get; }
    }
}