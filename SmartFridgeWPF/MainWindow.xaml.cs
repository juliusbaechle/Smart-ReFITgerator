using SmartFridgeWPF.ProductNS;
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

namespace SmartFridgeWPF
{
    public enum EPage
    {
        Home,
        Products,
        Content,
        Nutrition,
        Messages,
        Shopping
    }

    public partial class MainWindow : Window
    {
        public delegate void PageHandler(EPage page);
        public event PageHandler Open;

        public MainWindow()
        {
            InitializeComponent();
            
            btnHome.Click       += (object sender, RoutedEventArgs e) => { Open?.Invoke(EPage.Home); };
            btnProducts.Click   += (object sender, RoutedEventArgs e) => { Open?.Invoke(EPage.Products); };
            btnContent.Click    += (object sender, RoutedEventArgs e) => { Open?.Invoke(EPage.Content); };
            btnNutrition.Click  += (object sender, RoutedEventArgs e) => { Open?.Invoke(EPage.Nutrition); };
            btnMessages.Click   += (object sender, RoutedEventArgs e) => { Open?.Invoke(EPage.Messages); };
            btnShopping.Click   += (object sender, RoutedEventArgs e) => { Open?.Invoke(EPage.Shopping); };
            btnClose.Click      += (object sender, RoutedEventArgs e) => { Close(); };

            SetContent(new ProductForm());
        }

        public void SetConnectionInfo(string text)
        {
            txtConnection.Text = text;
        }

        public void SetTemperatureInfo(int degrees)
        {
            txtTemperature.Text = "Temperature: " + degrees + "°C";
        }

        public void SetHumidityInfo(int percent)
        {
            txtHumidity.Text = "Humidity: " + percent + "%";
        }

        public void SetContent(ContentControl page)
        {
            Content = page;
        }
    }
}
