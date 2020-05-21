using System;
using System.CodeDom;
using System.Collections.Generic;

namespace SmartFridge.ProductNS
{
    public enum ECategory
    {
        Drinks,
        Vegetable_Fruit,
        Dairy_Product,
        Meat_Fish_Eggs,
        Other
    }

    public enum EQuantity
    {
        Grams,
        Milliliters,
        Count
    }

    public class Product
    {
        internal Product()
        {
            ID = Guid.NewGuid().ToString("N").ToUpper();
            Category = ECategory.Drinks;
            Image = new Image();
        }

        internal Product(Product copy)
        {
            Name        = copy.Name;
            Energy      = copy.Energy;
            Durability  = copy.Durability;
            Category    = copy.Category;
            Quantity    = copy.Quantity;
            ID          = copy.ID;
            Image       = new Image(copy.Image);
        }

        internal bool ValueEqual(Product product)
        {
            if (product == null) return false;
            if (ID          != product.ID)          return false;
            if (Name        != product.Name)        return false;
            if (Energy      != product.Energy)      return false;
            if (Durability  != product.Durability)  return false;
            if (Category    != product.Category)    return false;
            if (Quantity    != product.Quantity)    return false;
            if (Image.ID    != product.Image.ID)    return false;
            return true;            
        }

        internal bool IsValid()
        {
            if (Name == "" || Name == null) return false;
            return true;
        }

        public string Name { set; get; }
        public UInt16 Energy { set; get; }
        public UInt16 Durability { set; get; }
        public ECategory Category { set; get; }
        public EQuantity Quantity { set; get; }
        internal string ID { set; get; }   
        public Image Image { internal set; get; }

        public static Dictionary<ECategory, string> CategoryCaptions { get; } =
            new Dictionary<ECategory, string>()
            {
                {ECategory.Drinks,          "Getränk" },
                {ECategory.Vegetable_Fruit, "Obst und Gemüse" },
                {ECategory.Dairy_Product,   "Milchprodukte" },
                {ECategory.Meat_Fish_Eggs,  "Fleisch, Fisch und Eier" },
                {ECategory.Other,           "Anderes" }
            };

        public static Dictionary<EQuantity, string> QuantityCaptions { get; } =
            new Dictionary<EQuantity, string>() {
                {EQuantity.Grams,       "Gramm" },
                {EQuantity.Milliliters, "Milliliter" },
                {EQuantity.Count,       "Anzahl" }
            };
    }
}
