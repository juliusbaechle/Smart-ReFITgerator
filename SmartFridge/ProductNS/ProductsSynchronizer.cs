using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Timers;
using System.Threading.Tasks;
using System.Net;

namespace SmartFridge.ProductNS
{
    internal class ProductsSynchronizer
    {
        private readonly Timer m_timer;
        private readonly Products m_products;
        
        private readonly DBProducts m_db;
        private readonly ImageRepository m_imgRepo;

        private readonly List<Product> m_deletedProducts = new List<Product>();
        private readonly List<Product> m_savedProducts = new List<Product>();
        private readonly List<Image> m_deletedImages = new List<Image>();
        private readonly List<Image> m_savedImages = new List<Image>();

        private const double INTERVAL = 5000;
        private readonly System.Threading.Mutex m_mutex = new System.Threading.Mutex();

        internal ProductsSynchronizer(Products products, DBProducts db, ImageRepository imgRepo)
        {
            m_db = db;
            m_imgRepo = imgRepo;

            m_products = products;
            m_products.Added   += m_savedProducts.Add;
            m_products.Updated += m_savedProducts.Add;
            m_products.Deleted += m_deletedProducts.Add;            
            m_products.SavedImg += image => m_savedImages.Add(image);
            m_products.DeletedImg += image => m_deletedImages.Add(image);

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
            m_deletedImages.Clear();
            m_savedImages.Clear();

            m_mutex.ReleaseMutex();
        }

        private void OpenConnection()
        {
            if (m_db.DbConnection.State != System.Data.ConnectionState.Open)
            {
                m_db.DbConnection.Open();
            }
        }

        private void Push()
        {
            m_deletedProducts.ForEach(m_db.Delete);
            m_savedProducts.ForEach(m_db.Save);

            m_deletedImages.ForEach(image => m_imgRepo.DeleteAsync(image).Wait());
            m_savedImages.ForEach(image => m_imgRepo.SaveAsync(image).Wait());
        }

        private void Pull()
        {
            var remoteProducts = m_db.LoadAll();
            var currentProducts = new List<Product>(m_products.List);
            var deletedProducts = currentProducts.Except(remoteProducts, new IdEqual());
            var changedOrCreatedProducts = remoteProducts.FindAll(remote => !remote.ValueEqual(currentProducts.Find(current => current.ID == remote.ID)));

            foreach(Product product in changedOrCreatedProducts)
                m_imgRepo.LoadAsync(product.Image).Wait();

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
