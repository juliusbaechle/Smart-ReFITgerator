using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SmartFridge.ProductNS
{
    public enum ECategory
    {
        None = -1,
        Drinks,
        Vegetable_Fruit,
        Cereal_Product,
        Dairy_Product,
        Meat_Fish_Eggs,
        Fat_Oil,
        Confectionery
    }

    public enum EQuantity
    {
        None,
        Grams,
        Milliliters,
        Count
    }

    public class Product : DependencyObject
    {
        public Product()
        {
            ID = Guid.NewGuid();
            Barcodes = new List<UInt64>();
            Category = ECategory.None;
        }

        public bool IsValid()
        {
            if (Name == "") return false;
            if (Category == ECategory.None) return false;
            if (Quantity == EQuantity.None) return false;
            return true;
        }

        public string Name { set; get; }
        public UInt16 Energy { set; get; }
        public UInt16 Durability { set; get; }
        public ECategory Category { set; get; }
        public EQuantity Quantity { set; get; }
        internal Guid ID { set; get; }
        internal List<UInt64> Barcodes { set; get; }
    }
}
