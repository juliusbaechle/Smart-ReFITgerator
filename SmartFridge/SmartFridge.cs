using SmartFridge.ProductNS;
using SmartFridgeWPF;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SmartFridge
{
    // Hier werden alle Objekte, Datenverbindungen, ... erzeugt
    // und im Rahmen der DependencyInjection übergeben

    class SmartFridge
    {   
        [System.STAThreadAttribute()]
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public static void Main()
        {
            SmartFridge.Setup();
        }

        private static void Setup()
        {
            Application app = Application.LoadComponent(new Uri("App.xaml", UriKind.Relative)) as Application;
            var mainWindow = new MainWindow(app.Resources);

            var db = DB.CreateLocalConnection();
            ImageRepository = new LocalImageRepository();

            var products = new Products(new DBProducts(db));            
            // var contents = new Contents(new DBContents(db));
            // ...
            
            var mediator = new Mediator(mainWindow, products);
            mediator.ShowPage(EPage.Home);    

            app.Run(mainWindow);
        }

        public static IImageRepository ImageRepository { get; private set; }
    }
}
