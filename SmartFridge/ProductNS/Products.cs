using System;
using System.ComponentModel;

namespace SmartFridge.ProductNS
{
    public class Products
    {
        public Action<Product> Selected;
        public Action<Product> Deleted;
        public Action<Product> Added;

        internal Products(DBProducts db)
        {
            m_db = db;
            List = new BindingList<Product>(m_db.LoadAll());
        }
         
        internal void AddOrEdit(Product newProduct)
        {
            if (!newProduct.IsValid()) return;

            var oldProduct = Get(newProduct.ID);
            if (oldProduct != null) 
                Delete(oldProduct);

            Add(newProduct);
        }

        internal void Delete(Product product)
        {
            List.Remove(product);
            m_db.Delete(product);
            Deleted?.Invoke(product);
        }

        internal void Add(Product product)
        {
            List.Add(product);
            m_db.Save(product);
            Added?.Invoke(product);
        }

        public Product Get(string productId)
        {
            Product oldProduct = null;
            foreach (Product product in List)
            {
                if (product.ID == productId)
                    oldProduct = product;
            }
            return oldProduct;
        }

        public BindingList<Product> List { get; private set; }
        private readonly DBProducts m_db;
    }
}
