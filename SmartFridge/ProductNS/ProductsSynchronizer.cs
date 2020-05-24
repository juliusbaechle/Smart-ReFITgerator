using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Timers;
using System.Threading.Tasks;
using System.Net;
using System;

namespace SmartFridge.ProductNS
{
    internal class ProductsSynchronizer
    {
        private readonly Timer m_timer;
        private readonly Products m_products;
        private readonly DBProducts m_local;
        private readonly DBProducts m_remote;

        private readonly List<Product> m_deletedProducts = new List<Product>();
        private readonly List<Product> m_savedProducts = new List<Product>();

        private const double INTERVAL = 5000;
        private readonly System.Threading.Mutex m_mutex = new System.Threading.Mutex();
        private List<Image> m_downloadQueue = new List<Image>();

        internal ProductsSynchronizer(Products products, DBProducts local, DBProducts remote)
        {
            m_local = local;
            m_remote = remote;

            m_products = products;
            m_products.Added += Save;
            m_products.Deleted += Delete;

            m_timer = new Timer();
            m_timer.Interval = INTERVAL;
            m_timer.Elapsed += (object o, ElapsedEventArgs e) => { Synchronize(); };
            m_timer.Start();
        }

        private void Synchronize()
        {
            if (!m_mutex.WaitOne(100)) return;

            if (IsConnected())
            {
                OpenConnection();
                Push();
                Pull();
            }

            m_deletedProducts.Clear();
            m_savedProducts.Clear();

            m_mutex.ReleaseMutex();
        }

        private void OpenConnection()
        {
            if (m_remote.DbConnection.State != System.Data.ConnectionState.Open)
            {
                m_remote.DbConnection.Open();
            }
        }

        private void Push()
        {
            Application.Current.Dispatcher.Invoke(() => {
                m_deletedProducts.ForEach(m_remote.Delete);
                m_savedProducts.ForEach(m_remote.Save);
            });
        }

        private void Pull()
        {
            var remoteProducts = m_remote.LoadAll();
            var currentProducts = new List<Product>(m_products.List);
            var deletedProducts = currentProducts.Except(remoteProducts, new IdEqual());
            var changedOrCreatedProducts = remoteProducts.FindAll(remote => !remote.ValueEqual(currentProducts.Find(current => current.ID == remote.ID)));

            //Bilder herunterladen
            foreach (Product product in changedOrCreatedProducts) {
                Task.Run(() => {
                    m_remote.ImageRepository.Load(product.Image);
                    m_local.ImageRepository.Save(product.Image);
                });
            }

            Application.Current.Dispatcher.Invoke(() => {
                foreach (Product product in deletedProducts)
                    m_products.Delete(product);

                foreach (Product product in changedOrCreatedProducts)
                    m_products.AddOrEdit(product);
            });
        }

        private bool IsConnected()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://google.com/generate_204"))
                    return true;
            }
            catch
            {
                return false;
            }
        }

        private void Save(Product product)
        {
            m_savedProducts.Add(product);
        }

        private void Delete(Product product)
        {
            m_deletedProducts.Add(product);
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
