using SmartFridgeWPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SmartFridge.ProductNS
{
    public delegate void ProductHandler(Product product);

    public partial class ProductOverview : Page
    {
        public event Action AddProduct;
        public event ProductHandler EditProduct;
        public event ProductHandler DeleteProduct;
        public event ProductHandler SelectedProduct;

        public ProductOverview()
        {
            InitializeComponent();
        }

        public ProductOverview(Products products)
        {
            InitializeComponent();
            listBoxProducts.ItemsSource = products.List;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddProduct?.Invoke();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var product = listBoxProducts.SelectedItem as Product;
            if (product == null) return;
            EditProduct?.Invoke(product);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var product = listBoxProducts.SelectedItem as Product;
            if (product == null) return;
            DeleteProduct?.Invoke(product);
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            var product = listBoxProducts.SelectedItem as Product;
            if (product == null) return;
            SelectedProduct?.Invoke(product);
        }
    }
}
