﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SmartFridge.ProductNS
{
    public enum ECategory
    {
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
        Grams,
        Milliliters,
        Count
    }

    public class Product : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public Product()
        {
            ID = Guid.NewGuid();
            Barcodes = new List<UInt64>();
            Category = ECategory.Drinks;
        }

        public bool IsValid()
        {
            if (Name == "") return false;
            return true;
        }

        public string Name { set; get; }
        public UInt16 Energy { set; get; }
        public UInt16 Durability { set; get; }
        public ECategory Category { set; get; }
        public EQuantity Quantity { set; get; }
        internal Guid ID { set; get; }
        internal List<UInt64> Barcodes { set; get; }
        
        public BitmapImage Image { 
            set {
                m_image = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Image"));
            } 
            get { return m_image; }
        }
        public string ImageId { get; set; }
        private BitmapImage m_image = null;    


        public static Dictionary<ECategory, string> CategoryCaptions { get; } =
            new Dictionary<ECategory, string>()
            {
                {ECategory.Drinks,          "Getränk" },
                {ECategory.Vegetable_Fruit, "Obst & Gemüse" },
                {ECategory.Cereal_Product,  "Getreideprodukt" },
                {ECategory.Dairy_Product,   "Milchprodukt" },
                {ECategory.Meat_Fish_Eggs,  "Fleisch & Fisch & Eier" },
                {ECategory.Fat_Oil,         "Fett & Öl" },
                {ECategory.Confectionery,   "Genussmittel" }
            };

        public static Dictionary<EQuantity, string> QuantityCaptions { get; } =
            new Dictionary<EQuantity, string>() {
                {EQuantity.Grams,       "Gramm" },
                {EQuantity.Milliliters, "Milliliter" },
                {EQuantity.Count,       "Anzahl" }
            };
    }
}
