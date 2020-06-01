using System;
using SmartFridge.ProductNS;

namespace SmartFridge.ContentNS
{
     public class Item
    {
        public Item()
        {
            ID = Guid.NewGuid().ToString("N").ToUpper();
            Product = null;
        }
        internal string ID { get; set; }

        internal string ProductID { get; set; }

        public DateTime ExpiryDate  { get; set; }

        public UInt32 Amount { get; set; }
        public Product Product { get; set; }

        public int Durability { 
            get { return (ExpiryDate - DateTime.Now).Days; } 
        }

        public uint Energy { 
            get {
                if (Product.Quantity == EQuantity.Count)
                    return Amount * Product.Energy;
                else
                    return Amount * Product.Energy / 100; 
            } 
        }

        internal bool IsValid()
        {
            if (Amount <= 0) return false;
            return true;
        }
    }
}
