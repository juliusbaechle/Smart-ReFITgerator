using System;
using System.ComponentModel;

namespace SmartFridge.ProductNS
{
    public class Products
    {
        public Action<Product> Selected;
        public Action<Product> Deleted;
        public Action<Product> Added;
        public Action<Product> Changed;

        public Action<Image> DeletedImg;
        public Action<Image> SavedImg;

        internal Products(DBProducts db, ImageRepository imageRepo)
        {
            m_db = db;
            m_imageRepo = imageRepo;

            List = new BindingList<Product>(m_db.LoadAll());            
            foreach (Product product in List) 
                m_imageRepo.LoadAsync(product.Image);
        }
         
        internal void AddOrEdit(Product newProduct)
        {
            if (!newProduct.IsValid()) return;

            var oldProduct = Get(newProduct.ID);
            if (oldProduct != null) 
                Update(oldProduct, newProduct);
            else
                Add(newProduct);
        }

        internal void Delete(Product product)
        {
            List.Remove(product);
            m_db.Delete(product);
            Deleted?.Invoke(product);
            m_imageRepo.DeleteAsync(product.Image);
            DeletedImg?.Invoke(product.Image);
        }

        internal void Update(Product oldProduct, Product newProduct)
        {
            List.Remove(oldProduct);
            List.Add(newProduct);

            if (oldProduct.Image.ID != newProduct.Image.ID) {
                m_imageRepo.DeleteAsync(oldProduct.Image);
                DeletedImg?.Invoke(oldProduct.Image);
                m_imageRepo.SaveAsync(newProduct.Image);
                SavedImg?.Invoke(newProduct.Image);
            }

            m_db.Save(newProduct);
            Changed?.Invoke(newProduct);
        }

        internal void Add(Product product)
        {
            List.Add(product);
            m_db.Save(product);
            Added?.Invoke(product);
            m_imageRepo.SaveAsync(product.Image);
            SavedImg?.Invoke(product.Image);
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
        private ImageRepository m_imageRepo;
    }
}
