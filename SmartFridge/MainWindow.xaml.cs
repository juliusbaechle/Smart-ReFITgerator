using SmartFridge;
using System.Windows;
using System.Windows.Controls;

namespace SmartFridgeWPF
{
    public partial class MainWindow : Window
    {
        public delegate void PageHandler(EPage page);
        public event PageHandler OpenPage;

        public MainWindow(ResourceDictionary resources)
        {
            Resources = resources;
            InitializeComponent();
            ConnectButtons();
        }

        private void ConnectButtons()
        {
            btnHome.Click       += (object sender, RoutedEventArgs e) => { OpenPage?.Invoke(EPage.Home); };
            btnProducts.Click   += (object sender, RoutedEventArgs e) => { OpenPage?.Invoke(EPage.Products); };
            btnContent.Click    += (object sender, RoutedEventArgs e) => { OpenPage?.Invoke(EPage.Content); };
            btnNutrition.Click  += (object sender, RoutedEventArgs e) => { OpenPage?.Invoke(EPage.Nutrition); };
            btnMessages.Click   += (object sender, RoutedEventArgs e) => { OpenPage?.Invoke(EPage.Messages); };
            btnShopping.Click   += (object sender, RoutedEventArgs e) => { OpenPage?.Invoke(EPage.Shopping); };
            btnClose.Click      += (object sender, RoutedEventArgs e) => { Close(); };
        }

        public void SetContent(Page page)
        {
            ContentFrame.NavigationService.RemoveBackEntry();
            ContentFrame.Content = page;
        }

        public void SetConnectionState(bool connected)
        {
            if (connected)
            {
                txtConnection.Text = "Verbunden";
            }
            else
            {
                txtConnection.Text = "Offline";
            }
        }
    }
}
