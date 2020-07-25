using SmartFridge.Arduino;
using System.Windows;
using System.Windows.Controls;

namespace SmartFridge
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
            if (connected) txtConnection.Text = "Online";
            else txtConnection.Text = "Offline";
        }


        public void SetDoorConnectionState(bool connected)
        {
            m_doorConnected = connected;
            if (!connected) txtDoor.Dispatcher.Invoke(() => { txtDoor.Text = "Kühlschrank nicht verbunden"; });
            else SetDoorState(false);            
        }
        private bool m_doorConnected = false;

        public void SetDoorState(bool open)
        {
            if (!m_doorConnected) return;
            if (open) txtDoor.Dispatcher.Invoke(() => { txtDoor.Text = "Tür offen"; });
            else txtDoor.Dispatcher.Invoke(() => { txtDoor.Text = "Tür geschlossen"; });
        }
    }
}
