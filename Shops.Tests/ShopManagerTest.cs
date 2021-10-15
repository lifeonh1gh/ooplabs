using System.Collections.Generic;
using NUnit.Framework;
using Shops.Controllers;
using Shops.Models;

namespace Shops.Tests
{
    public class Tests
    {
        private ShopManager _shopManager;
        private Product _product1;
        private Product _product2;
        private Product _product3;
        private Shop _shop1;
        private Shop _shop2;
        private Person _person;
        private List<ShopProduct> _shopProducts;
        private List<SupplyProduct> _supplyProducts;
        private List<PurchaseProduct> _purchaseProducts;


        [SetUp]
        public void Setup()
        {
            _shopManager = new ShopManager();
            _person = new Person("Rodion", 2000.0);
            _shop1 = _shopManager.CreateShop("IKEA", "Saint-Petersburg, Parnas");
            _shop2 = _shopManager.CreateShop("METRO", "Saint-Petersburg, Parnas");
            _product1 = _shopManager.RegisterProduct("Table");
            _product2 = _shopManager.RegisterProduct("Chair");
            _product3 = _shopManager.RegisterProduct("Lamp");
            _supplyProducts = new List<SupplyProduct>
            {
                new SupplyProduct(_product1, 100.0, 5),
                new SupplyProduct(_product2, 100.0, 5),
                new SupplyProduct(_product3, 100.0, 5)
            };
            _purchaseProducts = new List<PurchaseProduct>
            {
                new PurchaseProduct(_product1, 1), 
                new PurchaseProduct(_product2, 1), 
                new PurchaseProduct(_product3, 1)
            };
        }
        
        [Test]
        public void AddProductsToShop_ShopHasProductsAndProductsHasShop()
        {
            var expected = _shopManager.AddProducts(1, _supplyProducts);
            var actual = _shopManager.GetProducts(1);
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void ProductPriceChange_PriceChanged()
        {
            _shopManager.AddProducts(1, _supplyProducts);
            var expected = _shopManager.ChangePriceProduct(1, 1, 200);
            var actual = _shopManager.GetProduct(1);
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void BuyProductInShop_PersonTransferMoneyToShopAndProductAmountLess()
        {
            _shopManager.AddProducts(0, _supplyProducts);
            var expected = _shopManager.BuyProduct(_person, 0, 0, 1);
            var actual = _person.Money;
            Assert.AreEqual(expected, actual);
        }
        
        
        [Test]
        public void BuyConsignment_PersonTransferMoneyToShopAndProductsAmountLess()
        {
            _shopManager.AddProducts(0, _supplyProducts);
            var expected = _shopManager.BuyConsignment(_person, 0, _purchaseProducts);
            var actual = _person.Money;
            Assert.AreEqual(expected, actual);
        }
    }
}