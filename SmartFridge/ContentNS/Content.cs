using SmartFridge.ContentNS;
using SmartFridge.ProductNS;
using System;
using System.ComponentModel;

namespace SmartFridge.ContentNS
{
    public class Content
    {
        public event Action<Item> Added;
        public event Action<Item> Deleted;
        private Products m_products;
        internal Content(DBContent db1, Products products)
        {
            m_products = products;
            m_db = db1;
            List = new BindingList<Item>(m_db.LoadAll());
            foreach (Item item in List)
                item.Product = products.Get(item.ProductID);  
        }


        internal void AddOrEdit(Item newItem)
        {
            if (!newItem.IsValid()) return;

            var oldProduct = Get(newItem.ID);
            if (oldProduct != null)
                Delete(oldProduct);

            Add(newItem);
        }

        public Item Get(string ItemID)
        {
            Item oldItem = null;
            foreach (Item item in List)
            {
                if (item.ID == ItemID)
                    oldItem = item;
            }
            return oldItem;
        }

        internal void Add(Item item)
        {
            List.Add(item);
            m_db.Save(item);
            Added?.Invoke(item);
        }

        internal void Delete(Item item)
        {
            List.Remove(item);
            m_db.Delete(item);
            Deleted?.Invoke(item);
        }

        public BindingList<Item> List { get; private set; }
        private readonly DBContent m_db;

    }   
}
