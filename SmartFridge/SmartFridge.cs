using SmartFridge.ContentNS;
using SmartFridge.ProductNS;
using SmartFridgeWPF;
using System;
using System.Windows;

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
            var mainWindow = new MainWindow(app.Resources);

            var localDbProducts = new DBProducts(DB.CreateLocalConnection());
            var remoteDbProducts = new DBProducts(DB.CreateRemoteConnection());
            var products = new Products(localDbProducts, new LocalImageRepository());
            new ProductsSynchronizer(products, remoteDbProducts, new RemoteImageRepository());

            var localDbContent = new DBContent(DB.CreateLocalConnection());
            var remoteDbContent = new DBContent(DB.CreateRemoteConnection());
            var content = new Content(localDbContent, products);
            //new ContentSynchronizer(content, localDbContent, remoteDbContent);

            var mediator = new Mediator(mainWindow, products, content);
            mediator.ShowPage(EPage.Home);
            
            app.Run(mainWindow);
        }
    }
}
