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

            List = new BindingList<Product>(m_db.LoadAll());
            foreach (Product product in List) {
                product.Image.SetRepository(imageRepository);
                product.Image.Load();
            }
        }
         
        public void AddOrEdit(Product product)
        {
            if (!List.Contains(product))
                List.Add(product);

            product.Image.Save();
            m_db.Save(product);
        }

        public void Delete(Product product)
        {
            List.Remove(product);
            m_db.Delete(product);
            product.Image.Delete();
        }

        private readonly DBProducts m_db;
        public BindingList<Product> List { get; internal set; }
    }
}
