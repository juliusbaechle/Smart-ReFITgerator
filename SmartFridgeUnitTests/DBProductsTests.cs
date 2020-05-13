using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartFridge.ProductNS;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Data.Common;
using Microsoft.Data.Sqlite;

namespace SmartFridgeUnitTests
{
    [TestClass]
    public class DBProductsTests
    {
        private static DbConnection testDB;
        private static Product product;
        private const string PATH = "URI=file:test.db";

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            testDB = new SQLiteConnection(PATH);
            testDB.Open();

            product = new Product();
            product.Name = "Cheddar";
            product.Category = ECategory.Dairy_Product;
            product.Energy = 200;
            product.Durability = 50;
            product.ImageId = "ABCDEFGH.png";
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            testDB.Close();
            testDB.Dispose();
            File.Delete("test.db");
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
            Assert.AreEqual(product.ImageId,  loadedProduct.ImageId);
        }

        [TestMethod]
        public void SaveProduct_ChangedProduct_UpdatedProduct()
        {
            DBProducts dbProducts = new DBProducts(testDB);
            dbProducts.Clear();

            dbProducts.Save(product);

            product.Energy = 500;
            dbProducts.Save(product);

            List<Product> products = dbProducts.LoadAll();
            Assert.AreEqual(products.Count, 1);

            Product loadedProduct = products[0];
            Assert.AreEqual(product.Name, loadedProduct.Name);
        }
    }
}
