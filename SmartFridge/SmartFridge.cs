using SmartFridge.Arduino;
using SmartFridge.ContentNS;
using SmartFridge.Messages;
using SmartFridge.Messages.Channels;
using SmartFridge.ProductNS;
using System.Windows;
using System;

namespace SmartFridge
{
    // Hier werden alle Objekte, Datenverbindungen, ... erzeugt
    // und im Rahmen der DependencyInjection übergeben

    class SmartFridge
    {   
        [System.STAThreadAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public static void Main()
        {
            SmartFridge.Setup();
        }

        private static void Setup()
        {
            Application app = Application.LoadComponent(new Uri("App.xaml", UriKind.Relative)) as Application;

            var startupWindow = CreateStartupWindow();
            startupWindow.Show();

            var localDbProducts = new DBProducts(DB.CreateLocalConnection());
            var localImgRepo = new LocalImageRepository();
            var products = new Products(localDbProducts, localImgRepo);
            var remoteDbProducts = new DBProducts(DB.CreateRemoteConnection());            
            var productSynchronizer = new ProductsSynchronizer(products, remoteDbProducts, localImgRepo, new RemoteImageRepository());

            var localDbContent = new DBContent(DB.CreateLocalConnection());            
            var content = new Content(localDbContent, products);
            var remoteDbContent = new DBContent(DB.CreateRemoteConnection());
            var contentSynchronizer = new ContentSynchronizer(content, remoteDbContent);

            var synchronizer = new Synchronizer();
            synchronizer.Add(productSynchronizer);
            synchronizer.Add(contentSynchronizer);

            var arduino = new ArduinoDevice();
            SetupMessaging(arduino);

            var mainWindow = new MainWindow(app.Resources);
            synchronizer.ConnectionState += mainWindow.SetConnectionState;
            arduino.ConnectionChanged += mainWindow.SetDoorConnectionState;
            arduino.DoorStateChanged += mainWindow.SetDoorState;

            var mediator = new Mediator(mainWindow, products, content, arduino);
            mediator.ShowPage(EPage.Home);
            startupWindow.Close();
            app.Run(mainWindow);
        }

        private static Window CreateStartupWindow()
        {
            var startupWindow = new Window();
            startupWindow.Content = new ImagePage("home.jpg");
            startupWindow.WindowStyle = WindowStyle.None;

            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;
            startupWindow.Left = (screenWidth - startupWindow.Height) / 2;
            startupWindow.Top = (screenHeight - startupWindow.Width) / 2;

            return startupWindow;
        }

        private static void SetupMessaging(IDoor door)
        {
            var fridgeOpenChannel = new FridgeOpenChannel(door);
            var whatsAppMessenger = new WhatsAppMessenger("4915902600345");
            var smsMessenger      = new SMSMessenger("4915902600345");
            var emailMessenger    = new EMailMessenger("julius.baechle@yahoo.de");

            fridgeOpenChannel.Send += (IMessage msg) => {
                whatsAppMessenger.Send(msg);
                smsMessenger.Send(msg);
                emailMessenger.Send(msg);
            };
        }
    }
}
