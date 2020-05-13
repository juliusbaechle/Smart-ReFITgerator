using System.ComponentModel;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace SmartFridge.ProductNS
{
    public interface IImageRepository
    {
        string Save(BitmapImage image);
        BitmapImage Load(string path);
        void Delete(string path);
    }
}
