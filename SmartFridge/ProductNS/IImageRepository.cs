using System.Windows.Media.Imaging;

namespace SmartFridge.ProductNS
{
    public interface IImageRepository
    {
        string Save(BitmapSource image);
        BitmapSource Load(string id);
        void Delete(string id);
        bool Contains(string id);
    }
}
