using System;
using System.Collections.Generic;
using Shops.Models;
using Shops.Services;

namespace Shops
{
    internal static class Program
    {
        private static void Main()
        {
            var person = new Person("Rodion", 20000.0);
            var shopManager = new ShopManager();
            Console.WriteLine("\nCreated shop:");
            Shop ikea = shopManager.CreateShop("IKEA", "Saint-Petersburg, Parnas");
            Console.WriteLine($"\tId: {ikea.Id} \tName: {ikea.Name} \tAddress: {ikea.Address}");

            Console.WriteLine("\nCreated products:");
            Product table = shopManager.RegisterProduct("Table");
            Console.WriteLine($"\tId: {table.Id} \tName: {table.Name}");
            Product chair = shopManager.RegisterProduct("Chair");
            Console.WriteLine($"\tId: {chair.Id} \tName: {chair.Name}");
            Product lamp = shopManager.RegisterProduct("Lamp");
            Console.WriteLine($"\tId: {lamp.Id} \tName: {lamp.Name}");

            Console.WriteLine($"\nAdded products in shop {ikea.Name}:");
            var productToSupply = new List<SupplyProduct>
            {
                new SupplyProduct(table, 100.0, 5),
                new SupplyProduct(chair, 100.0, 5),
                new SupplyProduct(lamp, 100.0, 5),
            };
            shopManager.AddProducts(0, productToSupply);
            foreach (var pr in productToSupply)
            {
                Console.WriteLine($"\tId: {pr.Product.Id}\tProduct: {pr.Product.Name} \tPrice = {pr.Price} \tAmount = {pr.Amount}");
            }

            shopManager.BuyProduct(person, 0, 2, 1);
            Console.WriteLine($"\nPurchase of a consignment of the buyer's {person.Name}:");
            var productsToBuy = new List<PurchaseProduct>
            {
                new PurchaseProduct(table, 1),
                new PurchaseProduct(chair, 1),
                new PurchaseProduct(lamp, 1),
            };
            shopManager.BuyConsignment(person, 0, productsToBuy);
            shopManager.FindCheapestShop(productsToBuy);
        }
    }
}