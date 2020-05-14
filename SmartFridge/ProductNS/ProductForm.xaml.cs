using SmartFridge.ProductNS;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Media;
using System;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Media.Imaging;

namespace SmartFridgeWPF.ProductNS
{
    public partial class ProductForm : Page
    {
        public event Action<Product> Finished;
        public Action<Product, string> DroppedImage;

        public ProductForm(Product product) 
        {
            InitializeComponent();
            if(product != null)
                DataContext = new Product(product);
        }

        private void Confirm_Clicked(object sender, RoutedEventArgs e)
        {
            var product = DataContext as Product;
            if (product == null) return;
            Finished?.Invoke(product);
        }

        private void Image_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                Product product = DataContext as Product;
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                product.Image.Set(files[0]);
            }
        }
    }
}
