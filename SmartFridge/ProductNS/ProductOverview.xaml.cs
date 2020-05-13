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
        public event Action Add;
        public event ProductHandler Edit;
        public event ProductHandler Delete;
        public event ProductHandler Selected;

        public ProductOverview()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Add?.Invoke();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var product = listBoxProducts.SelectedItem as Product;
            if (product == null) return;
            Edit?.Invoke(product);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var product = listBoxProducts.SelectedItem as Product;
            if (product == null) return;
            Delete?.Invoke(product);
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            var product = listBoxProducts.SelectedItem as Product;
            if (product == null) return;
            Selected?.Invoke(product);
        }
    }
}
