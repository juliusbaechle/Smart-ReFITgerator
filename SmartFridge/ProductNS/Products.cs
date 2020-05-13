using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace SmartFridge.ProductNS
{
    public class Products
    {
        public ProductHandler Selected;

        internal Products(DBProducts db, IImageRepository imageRepository)
        {
            m_db = db;
            m_imageRepository = imageRepository;

            List = new BindingList<Product>(m_db.LoadAll());
            foreach (Product product in List) 
                product.Image = m_imageRepository.Load(product.ImageId);
        }

        public void SetImage(Product product, string path)
        {
            Uri uri = new Uri(path, UriKind.Absolute);
            BitmapImage bitmapImage = new BitmapImage(uri);
            product.Image = bitmapImage;
        }
         
        public void AddOrEdit(Product product)
        {
            if (!List.Contains(product))
                List.Add(product);

            string newId = m_imageRepository.Save(product.Image);
            m_imageRepository.Delete(product.ImageId);
            product.ImageId = newId;
            m_db.Save(product);
        }

        public void Delete(Product product)
        {
            List.Remove(product);
            m_db.Delete(product);
            m_imageRepository.Delete(product.ImageId);
        }

        private readonly DBProducts m_db;
        private readonly IImageRepository m_imageRepository;
        public BindingList<Product> List { get; internal set; }
    }
}
