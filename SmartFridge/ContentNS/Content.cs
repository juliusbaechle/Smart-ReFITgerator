﻿using SmartFridge.ContentNS;
using SmartFridge.ProductNS;
using System;
using System.Collections.Generic;
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
            m_products.Deleted += OnProductDeleted;
            m_products.Changed += OnProductUpdated;

            m_db = db1;
            List = new BindingList<Item>(m_db.LoadAll());
            foreach (Item item in List)
                item.Product = products.Get(item.ProductID);  
        }


        internal void AddOrEdit(Item newItem)
        {
            if (!newItem.IsValid()) return;
            if (m_products.Get(newItem.ProductID) == null) return;

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
            item.Product = m_products.Get(item.ProductID);
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

        private void OnProductDeleted(Product product)
        {
            Item deleteItem = null;
            foreach (Item item in List)
                if (item.Product == product)
                    deleteItem = item; 
            Delete(deleteItem);
        }

        private void OnProductUpdated(Product product)
        {
            List<Item> updatedItems = new List<Item>();
            foreach (Item item in List) {
                if (item.ProductID == product.ID) {
                    updatedItems.Add(item);
                }
            }

            updatedItems.ForEach(item => {
                item.Product = product;

                m_db.Save(item);
                Added?.Invoke(item);

                // Refresh UI
                List.Remove(item);
                List.Add(item);
            });
        }

        public BindingList<Item> List { get; private set; }

        private readonly DBContent m_db;
    }   
}
