using System;
using System.ComponentModel;

namespace SmartFridge.ProductNS
{
    public class Products
    {
        public ProductHandler Selected;

        internal Products(DBProducts db, ImageRepository imageRepository)
        {
            m_db = db;
            m_imageRepository = imageRepository;

            List = new BindingList<Product>(m_db.LoadAll());
            foreach (Product product in List) {
                m_imageRepository.LoadAsync(product.Image);
            }
        }
         
        internal void AddOrEdit(Product newProduct)
        {
            if (!newProduct.IsValid()) return;

            Product oldProduct = Find(newProduct.ID);
            if (oldProduct != null)
            {
                List.Remove(oldProduct);
                m_imageRepository.DeleteAsync(oldProduct.Image);
            }

            List.Add(newProduct);
            m_imageRepository.SaveAsync(newProduct.Image);
            m_db.Save(newProduct);
        }

        internal void Delete(Product product)
        {
            List.Remove(product);
            m_db.Delete(product);
            m_imageRepository.DeleteAsync(product.Image);
        }

        public Product Find(Guid id)
        {
            Product oldProduct = null;
            foreach (Product product in List)
            {
                if (product.ID == id)
                    oldProduct = product;
            }
            return oldProduct;
        }

        public BindingList<Product> List { get; private set; }
        private readonly DBProducts m_db;
        ImageRepository m_imageRepository;
    }
}
