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
            foreach (Product product in List) {
                product.Image.Load(m_imageRepository);
            }
        }
         
        public void AddOrEdit(Product newProduct)
        {
            if (!newProduct.IsValid()) return;
            
            Delete(newProduct.ID);
            List.Add(newProduct);
            newProduct.Image.Save(m_imageRepository);
            m_db.Save(newProduct);
        }

        public void Delete(Product product)
        {
            List.Remove(product);
            m_db.Delete(product);
            product.Image.Delete(m_imageRepository);
        }

        private void Delete(Guid id)
        {
            Product oldProduct = null;
            foreach (Product product in List)
            {
                if (product.ID == id)
                {
                    oldProduct = product;
                }
            }

            if (oldProduct != null)
            {
                List.Remove(oldProduct);
                oldProduct.Image.Delete(m_imageRepository);
            }
        }

        private readonly DBProducts m_db;
        public BindingList<Product> List { get; internal set; }
        IImageRepository m_imageRepository;
    }
}
