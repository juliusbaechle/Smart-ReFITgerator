using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartFridge.Products;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace SmartFridgeUnitTests
{
    [TestClass]
    public class DBProductsTests
    {
        private static SQLiteConnection testDB;
        private static Product product;
        private const string PATH = "URI=file:test.db";

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            testDB = new SQLiteConnection(PATH);
            testDB.Open();

            product = new Product();
            product.Name = "Cheddar";
            product.Category = EProductCategory.Dairy_Product;
            product.Energy = 200;
            product.Durability = 50;
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            testDB.Dispose();
            File.Delete("URI=file:test.db");
        }

        [TestMethod]
        public void SaveProduct_NewProduct_SavedEqualsLoadedProduct()
        {
            DBProducts dbProducts = new DBProducts(testDB);
            dbProducts.Clear();


            dbProducts.Save(product);


            List<Product> products = dbProducts.LoadAll();
            Assert.AreEqual(products.Count, 1);

            Product loadedProduct = products[0];
            Assert.AreEqual(product.ID,         loadedProduct.ID);
            Assert.AreEqual(product.Name,       loadedProduct.Name);
            Assert.AreEqual(product.Durability, loadedProduct.Durability);
            Assert.AreEqual(product.Energy,     loadedProduct.Energy);
            Assert.AreEqual(product.Category,   loadedProduct.Category);
        }

        [TestMethod]
        public void SaveProduct_ChangedProduct_UpdatedProduct()
        {
            string path = "URI=file:test.db";
            SQLiteConnection testDB = new SQLiteConnection(path);
            testDB.Open();

            DBProducts dbProducts = new DBProducts(testDB);
            dbProducts.Clear();

            dbProducts.Save(product);

            product.Energy = 500;
            dbProducts.Save(product);

            List<Product> products = dbProducts.LoadAll();
            Assert.AreEqual(products.Count, 1);

            Product loadedProduct = products[0];
            Assert.AreEqual(product.Energy, loadedProduct.Energy);
        }
    }
}