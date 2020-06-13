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

        public Item(Item copyFrom)
        {
            ID          = copyFrom.ID;
            ExpiryDate  = copyFrom.ExpiryDate;
            Amount      = copyFrom.Amount;
            Product     = copyFrom.Product;
            ProductID   = copyFrom.ProductID;
        }

        internal string ID { get; set; }

        internal string ProductID { get; set; }

        public DateTime ExpiryDate  { get; internal set; }

        public UInt32 Amount { get; internal set; }
        public Product Product { get; internal set; }

        public string AmountText {
            get {
                switch (Product.Quantity) {
                    case EQuantity.Count:
                        return Amount + " %";
                    case EQuantity.Milliliters:
                        return Amount + " ml";
                    default:
                        return Amount + " g";
                }            
            }
        }

        public int Durability { 
            get { return (ExpiryDate - DateTime.Now).Days + 1; } 
        }

        public uint Energy { 
            get { return Amount * Product.Energy / 100; } 
        }
        
        internal bool IsValid()
        {
            if (Amount <= 0) return false;
            return true;
        }
    }
}
