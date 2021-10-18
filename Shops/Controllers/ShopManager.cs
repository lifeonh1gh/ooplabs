using System;
using System.Collections.Generic;
using System.Linq;
using Shops.Interfaces;
using Shops.Models;
using Shops.Tools;

namespace Shops.Controllers
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
            try
            {
                ShopProduct shopProduct = ShopProducts.First(sp => sp.Product.Id == productId);
                return shopProduct;
            }
            catch (Exception e)
            {
                throw new ShopsException(e.Message);
            }
        }

        public Shop GetShop(int shopId)
        {
            try
            {
                ShopProduct shopProduct = ShopProducts.First(sp => sp.Shop.Id == shopId);
                return shopProduct.Shop;
            }
            catch (Exception e)
            {
                throw new ShopsException(e.Message);
            }
        }

        public ShopProduct GetProducts(int shopId)
        {
            try
            {
                ShopProduct shopProducts = ShopProducts.First(sp => sp.Shop.Id == shopId);
                return shopProducts;
            }
            catch (Exception e)
            {
                throw new ShopsException(e.Message);
            }
        }

        public ShopProduct AddProducts(int shopId, List<SupplyProduct> products)
        {
            var shop = Shops[shopId];
            foreach (var sp in products.Select(product =>
                new ShopProduct()
                    { Shop = shop, Product = product.Product, Amount = product.Amount, Price = product.Price }))
            {
                ShopProducts.Add(sp);
            }

            return GetProducts(shopId);
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
            var shop = Shops[shopId];
            var product = Products[productId];
            var element = ShopProducts.ElementAt(productId);
            var amount = element.Amount;
            try
            {
                ShopProducts.RemoveAll(sp => sp.Product.Id == productId);
                ShopProduct shopProduct = new ShopProduct()
                    { Shop = shop, Product = product, Amount = amount, Price = newPrice };
                ShopProducts.Add(shopProduct);
                return shopProduct;
            }
            catch (Exception e)
            {
                throw new ShopsException("Price amount change error for product", e);
            }
        }

        public double BuyProduct(Person person, int shopId, int productId, int amount)
        {
            var totalPrice = 0.0;
            foreach (var sp in ShopProducts.Where(sp => sp.Shop.Id == shopId && sp.Product.Id == productId))
            {
                if (sp.Amount >= amount && sp.Amount > 0 && sp.Price * amount <= person.Money && person.Money > 0)
                {
                    sp.Amount -= amount;
                    totalPrice += sp.Price * amount;
                }
                else
                {
                    throw new ShopsException("Not enough money or not enough product");
                }
            }

            return person.Money -= totalPrice;
        }

        public double BuyConsignment(Person person, int shopId, List<PurchaseProduct> products)
        {
            var element = ShopProducts.ElementAt(shopId);
            var totalPrice = 0.0;
            var productPrice = element.Price;
            person.Money -= totalPrice;
            foreach (var purchaseProduct in products)
            {
                if (purchaseProduct.Amount > 0 && person.Money >= totalPrice)
                {
                    BuyProduct(person, shopId, purchaseProduct.Product.Id, purchaseProduct.Amount);
                    totalPrice += productPrice * purchaseProduct.Amount;
                }
                else
                {
                    throw new ShopsException("Not enough money or not enough products");
                }
            }

            return person.Money;
        }

        public Shop FindCheapShop(List<PurchaseProduct> products)
        {
            var product = products[Index.Start].Product;
            ShopProduct cheap = new ShopProduct() { Price = double.MaxValue };
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