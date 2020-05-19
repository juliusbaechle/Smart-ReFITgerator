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

            var localDbProducts = new DBProducts(DB.CreateLocalConnection(), new LocalImageRepository());
            var products = new Products(localDbProducts);
            var remoteDbProducts = new DBProducts(DB.CreateRemoteConnection(), new RemoteImageRepository());            
            new ProductsSynchronizer(products, localDbProducts, remoteDbProducts);

            // var content = new Contents(new DBContents(db));
            // ...

            var mediator = new Mediator(mainWindow, products);
            mediator.ShowPage(EPage.Home);
            
            app.Run(mainWindow);
        }
    }
}
