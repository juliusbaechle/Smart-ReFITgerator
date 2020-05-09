using System.Windows;

namespace SmartFridgeWPF
{
    class SmartFridge
    {
        [System.STAThreadAttribute()]
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public static void Main()
        {
            MainWindow window = new MainWindow();
            Application app = new Application();
            app.Run(window);
        }
    }
}
