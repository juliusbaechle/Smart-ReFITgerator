using System.ComponentModel;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace SmartFridge.ProductNS
{
    public interface IImageRepository
    {
        BitmapImage Load(string id);
        string Save(BitmapImage path);
        void Delete(string id);
    }
}
