using SmartFridge.ProductNS;
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
using System.Windows.Shapes;

namespace SmartFridgeWPF.ProductNS
{
    public partial class ProductForm : Frame
    {
        public delegate void ProductHandler (Product product);
        public event ProductHandler Finished;

        public ProductForm()
        {
            DataContext = new Product();
            InitializeComponent();            
        } 

        public ProductForm(Product product)
        {
            DataContext = product;
            InitializeComponent();            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var product = DataContext as Product;
            if (product == null || !product.IsValid()) return;
            Finished?.Invoke(product);
        }
    }
}
