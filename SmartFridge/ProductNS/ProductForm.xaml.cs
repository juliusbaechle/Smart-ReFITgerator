using SmartFridge.ProductNS;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Media;
using System;
using System.Windows.Data;

namespace SmartFridgeWPF.ProductNS
{
    public partial class ProductForm : Page
    {
        public delegate void ProductHandler (Product product);
        public event Action<Product> Finished;

        public ProductForm()
        {           
            InitializeComponent();
            (DataContext as Product).Image.Changed += Update;
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
                product.SetImage(files[0]);
            }
        }

        private void Update(object sender, EventArgs e)
        {

        }
    }
}
