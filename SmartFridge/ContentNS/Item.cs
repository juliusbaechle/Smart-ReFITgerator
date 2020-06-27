using System;
using System.Windows.Media.Imaging;
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

        internal bool ValueEqual(Item item)
        {
            if (item == null) return false;
            if (ID          != item.ID) return false;
            if (ExpiryDate  != item.ExpiryDate) return false;
            if (Amount      != item.Amount) return false;
            if (ProductID   != item.ProductID) return false;
            return true;
        }

        internal string ID { get; set; }
        public DateTime ExpiryDate  { get; internal set; }
        public UInt32 Amount { get; internal set; }

        public Product Product { get; internal set; }
        public string ProductID { get; set; }
        public BitmapSource Image { get { return Product.Image.Bitmap; } }
        public string Name { get { return Product.Name; } }
        public ECategory Category { get { return Product.Category; } }


        public string Unit {
            get {
                switch (Product.Quantity) {
                    case EQuantity.Count: return "%";
                    case EQuantity.Milliliters: return "ml";
                    default: return "g";
                }
            }
        }

        public string AmountText {
            get { return Amount + " " + Unit; }
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
