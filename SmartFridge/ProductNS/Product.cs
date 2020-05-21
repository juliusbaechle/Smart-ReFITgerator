using System;
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
        public Product()
        {
            ID = Guid.NewGuid();
            Category = ECategory.Drinks;
            Image = new ProductImage();
        }

        public Product(Product copy)
        {
            Name        = copy.Name;
            Energy      = copy.Energy;
            Durability  = copy.Durability;
            Category    = copy.Category;
            Quantity    = copy.Quantity;
            ID          = copy.ID;
            Image       = new ProductImage(copy.Image);
        }

        public bool IsValid()
        {
            if (Name == "" || Name == null) return false;
            return true;
        }

        public string Name { set; get; }
        public UInt16 Energy { set; get; }
        public UInt16 Durability { set; get; }
        public ECategory Category { set; get; }
        public EQuantity Quantity { set; get; }
        internal Guid ID { set; get; }   
        public ProductImage Image { set; get; }

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
