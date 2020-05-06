using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFridge.ProductNS
{
    public class Products
    {
        public Action Changed;

        internal Products(DBProducts db)
        {
            m_db = db;
            m_products = m_db.LoadAll();
        }

        public void AddOrEdit(Product product)
        {
            if (m_products.Contains(product))
            {
                m_products.Add(product);
            }

            m_db.Save(product);
        }

        private DBProducts m_db;
        private List<Product> m_products;
    }
}
