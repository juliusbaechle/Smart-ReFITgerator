using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Windows;
using System.Timers;
using System.Threading.Tasks;

namespace SmartFridge.ProductNS
{
    internal class ProductsSynchronizer
    {
        private readonly Timer m_timer;
        private readonly Products m_products;
        private readonly DBProducts m_local;
        private readonly DBProducts m_remote;

        private bool m_connected = true;

        private const double INTERVAL = 5000;

        internal ProductsSynchronizer(Products products, DBProducts local, DBProducts remote)
        {
            m_local = local;
            m_remote = remote;
            m_products = products;
            ConnectSignals();

            m_timer = new Timer();
            m_timer.Interval = INTERVAL;
            m_timer.Elapsed += (object o, ElapsedEventArgs e) => { Synchronize(); };
            m_timer.Start();
        }

        private void Synchronize()
        {
            var remoteProducts = m_remote.LoadAll();
            var currentProducts = new List<Product>(m_products.List);
            var deletedProducts = currentProducts.Except(remoteProducts, new IdEqual());
            var changedOrCreatedProducts = remoteProducts.FindAll(remote => !remote.ValueEqual(currentProducts.Find(current => current.ID == remote.ID)));

            Application.Current.Dispatcher.Invoke(() => {
                // Products emitted in den Methoden AddOrEdit und Delete Signale, 
                // diese kommen jedoch bereits aus der Remotedatenbank und müssen 
                // deshalb nicht hochgeladen werden
                DisconnectSignals();

                foreach (Product product in deletedProducts)
                    m_products.Delete(product);

                foreach (Product product in changedOrCreatedProducts)
                {
                    m_products.AddOrEdit(product);

                    Task.Run(() => {
                        // Bilder herunterladen und lokal abspeichern
                        m_remote.ImageRepository.LoadAsync(product.Image).Wait();
                        m_local.ImageRepository.SaveAsync(product.Image);
                    });
                }

                ConnectSignals();
            });
        }

        private void Save(Product product)
        {
            if (m_connected)
                m_remote.Save(product);
        }

        private void Delete(Product product)
        {
            if(m_connected)
                m_remote.Delete(product);
        }

        private void ConnectSignals()
        {
            m_products.Added += Save;
            m_products.Deleted += Delete;
        }

        private void DisconnectSignals()
        {
            m_products.Added -= Save;
            m_products.Deleted -= Delete;
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
