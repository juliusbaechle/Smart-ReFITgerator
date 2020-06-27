using System.Windows;
using System.Windows.Controls;
using System;
using Microsoft.Win32;

namespace SmartFridge.ProductNS
{
    public partial class ProductForm : Page
    {
        public event Action<Product> Finished;
        public Action<Product, string> DroppedImage;

        public ProductForm() 
        {
            InitializeComponent();
        }

        private void Confirm_Clicked(object sender, RoutedEventArgs e)
        {
            var product = DataContext as Product;
            if (product == null) return;
            Finished?.Invoke(product);
        }

        private void SearchFile_Clicked(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Bilder (*.BMP;*.JPG;*.GIF,*.PNG,*.TIFF)|*.BMP;*.JPG;*.GIF;*.PNG;*.TIFF";
            dialog.ShowDialog();

            Product product = DataContext as Product;
            product.Image.Set(dialog.FileName);
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
