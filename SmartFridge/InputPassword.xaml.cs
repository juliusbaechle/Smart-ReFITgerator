using System.Windows;

namespace SmartFridge
{
    /// <summary>
    /// Interaktionslogik für InputPassword.xaml
    /// </summary>
    public partial class InputPassword : Window
    {
        public InputPassword()
        {
            InitializeComponent();
        }

        public string Password {
            get { return txtPassword.Password; } 
        }

        private void OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
