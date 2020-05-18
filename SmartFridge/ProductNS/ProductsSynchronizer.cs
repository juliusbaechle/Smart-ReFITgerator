using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Windows.Threading;

namespace SmartFridge.ProductNS
{
    internal class ProductsSynchronizer
    {
        private readonly DispatcherTimer m_timer;
        private readonly Products m_products;
        private readonly DbConnection m_local;
        private readonly DbConnection m_remote;

        internal ProductsSynchronizer(Products products, DbConnection local, DbConnection remote)
        {
            m_products = products;
            m_local = local;
            m_remote = remote;

            m_timer = new DispatcherTimer();
            m_timer.Interval = new TimeSpan(0, 0, 10);
            m_timer.Tick += (object o, EventArgs e) => { Synchronize(); };
            m_timer.Start();
        }

        private void Synchronize()
        {
            Console.WriteLine(System.DateTime.Now.Second + ", " + System.DateTime.Now.Millisecond);
            var remoteDb = new DBProducts(m_remote);
            var remoteProducts = remoteDb.LoadAll();
            
            CopyToLocalDb(remoteProducts);
            SynchronizeProducts(remoteProducts);
            Console.WriteLine(System.DateTime.Now.Second + ", " + System.DateTime.Now.Millisecond + System.Environment.NewLine);
        }

        private void CopyToLocalDb(List<Product> products)
        {
            var localDb = new DBProducts(m_local);
            localDb.Clear();
            products.ForEach(localDb.Save);
        }

        private void SynchronizeProducts(List<Product> remoteProducts)
        {
            var currentProducts = new List<Product>(m_products.List);

            var deletedProducts = currentProducts.Except(remoteProducts, new IdEqual());
            foreach (Product product in deletedProducts) 
                m_products.Delete(product);

            var changedOrCreatedProducts = remoteProducts.FindAll(remote => !remote.ValueEqual(currentProducts.Find(current => current.ID == remote.ID)));
            foreach (Product product in changedOrCreatedProducts) 
                m_products.AddOrEdit(product);
        }

        class IdEqual : IEqualityComparer<Product>
        {
            public bool Equals(Product x, Product y)
            {
                return x.ID == y.ID;
            }

            public int GetHashCode(Product obj)
            {
                return obj.ID.GetHashCode();
            }
        }
    }
}
