using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFridge.ProductNS
{
    public enum ECategory
    {
        None,
        Fruit,       
        Vegetable,
        Meat_Fish,
        Dairy_Product,
        Breakfast,
        Other
    }

    public enum EQuantity
    {
        None,
        Grams,
        Milliliters,
        Count
    }

    public class Product
    {
        public Product()
        {
            ID = Guid.NewGuid();
            Barcodes = new List<UInt64>();            
        }

        public bool IsValid()
        {
            if (Name == "") return false;
            if (Category == ECategory.None) return false;
            if (Quantity == EQuantity.None) return false;
            return true;
        }

        public string Name {
            get;
            internal set;
        }

        public UInt16 Energy {
            get;
            internal set;
        }

        public UInt16 Durability {
            get;
            internal set;
        }

        public ECategory Category {
            get;
            internal set;
        }

        public EQuantity Quantity
        {
            get;
            internal set;
        }

        internal Guid ID {
            get;
            set;
        }

        internal List<UInt64> Barcodes {
            get;
            set;
        }
    }
}
