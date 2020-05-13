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
            if(product != null) DataContext = product;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var product = DataContext as Product;
            if (product == null || !product.IsValid()) return;
            Finished?.Invoke(product);
        }

        private void Image_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                Product product = DataContext as Product;
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                string path = files[0];

                var ImageExtensions = new List<string> { ".JPG", ".JPE", ".BMP", ".GIF", ".PNG" };
                if (!ImageExtensions.Contains(Path.GetExtension(path).ToUpperInvariant())) return;

                var uri = new Uri(path, UriKind.Absolute);
                var bitmapImage = new BitmapImage(uri);
                product.Image = bitmapImage;
            }
        }
    }
}
