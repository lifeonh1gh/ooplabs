using System.Collections.Generic;
using Shops.Models;

namespace Shops.Interfaces
{
    public interface IShopManager
    {
        Shop CreateShop(string name, string address);
        Product RegisterProduct(string name);
        ShopProduct AddProducts(int shopId, List<SupplyProduct> products);
        ShopProduct GetShopProduct(int shopId, int productId);
        List<ShopProduct> GetProductsInfo(int shopId);
        ShopProduct CheckProductsInShop(int shopId, List<SupplyProduct> products);
        ShopProduct ChangePriceProduct(int shopId, int productId, double newPrice);
        double BuyProduct(Person person, int shopId, int productId, int amount);
        double BuyConsignment(Person person, int shopId, List<PurchaseProduct> products);
        ShopProduct FindCheapShop(List<PurchaseProduct> products);
    }
}