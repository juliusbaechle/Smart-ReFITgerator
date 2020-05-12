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
            List = m_db.LoadAll();
        }
         
        public void AddOrEdit(Product product)
        {
            if (!List.Contains(product))
            {
                List.Add(product);
            }

            m_db.Save(product);
        }

        private DBProducts m_db;
        public List<Product> List { get; private set; }
    }
}
