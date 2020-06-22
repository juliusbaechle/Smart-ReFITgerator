using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace SmartFridge
{
    /// <summary>
    /// Interaktionslogik für ImagePage.xaml
    /// </summary>
    public partial class ImagePage : Page
    {
        public ImagePage(string imagePath)
        {
            InitializeComponent();

            #if DEBUG
                imagePath = "../../pageImages/" + imagePath;
            #else
                imagePath = "pageImages/" + imagePath;
            #endif

            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.CacheOption = BitmapCacheOption.OnLoad;
            img.UriSource = new Uri(imagePath, UriKind.Relative);
            img.EndInit();
            img.Freeze();
            BackgroundImage.Source = img;
        }
    }
}
