using System;
using System.Collections.Generic;
using System.Linq;
using Shops.Interfaces;
using Shops.Models;
using Shops.Tools;

namespace Shops.Services
{
    public class ShopManager : IShopManager
    {
        private List<Shop> Shops { get; } = new List<Shop>();
        private List<Product> Products { get; } = new List<Product>();
        private List<ShopProduct> ShopProducts { get; } = new List<ShopProduct>();

        public Shop CreateShop(string name, string address)
        {
            var shop = new Shop(Shops.Count, name, address);
            Shops.Add(shop);
            return shop;
        }

        public Product RegisterProduct(string name)
        {
            var product = new Product(Products.Count, name);
            Products.Add(product);
            return product;
        }

        public ShopProduct GetProduct(int productId)
        {
            var shopProduct = ShopProducts.FirstOrDefault(sp => sp.Product.Id == productId) ??
                              throw new ShopsException("Product not found");
            return shopProduct;
        }

        public Shop GetShop(int shopId)
        {
            var shop = Shops.FirstOrDefault(s => s.Id == shopId) ??
                       throw new ShopsException("Shop not found");
            return shop;
        }

        public ShopProduct AddProducts(int shopId, List<SupplyProduct> products)
        {
            var shop = Shops.FirstOrDefault(s => s.Id == shopId);
            foreach (var sp in products.Select(product =>
                new ShopProduct()
                    { Shop = shop, Product = product.Product, Amount = product.Amount, Price = product.Price }))
            {
                ShopProducts.Add(sp);
            }

            return GetProduct(shopId);
        }

        public ShopProduct GetShopProduct(int shopId, int productId)
        {
            var result = ShopProducts.Find(sp => sp.Shop.Id == shopId && sp.Product.Id == productId);
            if (result == null)
            {
                throw new ShopsException("No such Product in Shop");
            }

            return result;
        }

        public ShopProduct ChangePriceProduct(int shopId, int productId, double newPrice)
        {
            var shop = Shops.FirstOrDefault(s => s.Id == shopId);
            var product = Products.FirstOrDefault(p => p.Id == productId);
            var element = ShopProducts.ElementAt(productId);
            var amount = element.Amount;
            if (product == null)
            {
                throw new ShopsException("Price amount change error for product");
            }

            ShopProducts.RemoveAll(sp => sp.Product.Id == productId);
            var shopProduct = new ShopProduct()
                { Shop = shop, Product = product, Amount = amount, Price = newPrice };
            ShopProducts.Add(shopProduct);
            return shopProduct;
        }

        public double BuyProduct(Person person, int shopId, int productId, int amount)
        {
            var totalPrice = 0.0;
            foreach (var sp in ShopProducts.Where(sp => sp.Shop.Id == shopId && sp.Product.Id == productId))
            {
                if (sp.Amount > amount && sp.Amount > 0)
                {
                    sp.Amount -= amount;
                }
                else
                {
                    throw new ShopsException("Not enough product in shop");
                }

                if (sp.Price <= person.Money)
                {
                    totalPrice += sp.Price * amount;
                }
                else
                {
                    throw new ShopsException("Not enough money");
                }

                Console.WriteLine($"{sp.Product.Name}, {sp.Product.Id}, {sp.Amount}");
                return person.Money -= totalPrice;
            }

            throw new ShopsException("Product does not exist in shop");
        }

        public double BuyConsignment(Person person, int shopId, List<PurchaseProduct> products)
        {
            var element = ShopProducts.Where(sp => sp.Shop.Id == shopId).ElementAt(shopId);
            var totalPrice = 0.0;
            var productPrice = element.Price;
            person.Money -= totalPrice;
            foreach (var purchaseProduct in products)
            {
                if (purchaseProduct.Amount <= 0)
                {
                    throw new ShopsException($"Not enough product in shop");
                }
                else if (person.Money <= totalPrice)
                {
                    throw new ShopsException("Not enough money");
                }
                else
                {
                    BuyProduct(person, shopId, purchaseProduct.Product.Id, purchaseProduct.Amount);
                    totalPrice += productPrice * purchaseProduct.Amount;
                }
            }

            return person.Money;
        }

        public Shop FindCheapestShop(List<PurchaseProduct> products)
        {
            var product = products[0].Product;

            var cheap = new ShopProduct() { Price = double.MaxValue };
            foreach (var sp in ShopProducts)
            {
                if (sp.Product == product && sp.Price < cheap.Price)
                {
                    cheap = sp;
                }
            }

            return cheap.Shop;
        }
    }
}