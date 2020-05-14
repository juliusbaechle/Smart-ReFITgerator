using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFridge.ProductNS
{
    public class Products
    {
        public ProductHandler Selected;

        internal Products(DBProducts db)
        {
            m_db = db;
            List = new BindingList<Product>(m_db.LoadAll());
        }
         
        public void AddOrEdit(Product product)
        {
            if (!product.IsValid()) return;

            if (!List.Contains(product))
            {
                List.Add(product);
            }

            m_db.Save(product);
        }

        public void Delete(Product product)
        {
            List.Remove(product);
            m_db.Delete(product);
        }

        private DBProducts m_db;
        public BindingList<Product> List { get; internal set; }
    }
}
