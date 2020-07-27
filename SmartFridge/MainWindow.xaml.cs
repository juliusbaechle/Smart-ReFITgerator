using Renci.SshNet.Messages;
using SmartFridge.Arduino;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualBasic;

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
            btnClose.Click      += (object sender, RoutedEventArgs e) => { SwitchAccount(); }; 
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
            if (!connected) txtDoor.Dispatcher.Invoke(() => { txtDoor.Text = "Tür nicht verbunden"; });
            else SetDoorState(false);            
        }
        private bool m_doorConnected = false;

        public void SetDoorState(bool open)
        {
            if (!m_doorConnected) return;
            if (open) txtDoor.Dispatcher.Invoke(() => { txtDoor.Text = "Tür offen"; });
            else txtDoor.Dispatcher.Invoke(() => { txtDoor.Text = "Tür geschlossen"; });
        }


        [DllImport("wtsapi32.dll")]
        static extern bool WTSDisconnectSession(IntPtr hServer, int sessionId, bool bWait);
        const int WTS_CURRENT_SESSION = -1;
        static readonly IntPtr WTS_CURRENT_SERVER_HANDLE = IntPtr.Zero;

        private void SwitchAccount()
        {
            var dialog = new InputPassword();
            dialog.ShowDialog();
            if (dialog.Password != "SmartFIT") return;

            if (!WTSDisconnectSession(WTS_CURRENT_SERVER_HANDLE, WTS_CURRENT_SESSION, false))
                throw new Win32Exception();
        }
    }
}
