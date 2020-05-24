using System;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace SmartFridge.ProductNS
{
    public abstract class ImageRepository
    {
        internal abstract Task SaveAsync(Image image);
        public abstract Task LoadAsync(Image image);
        public abstract Task DeleteAsync(Image image);
        public abstract bool Contains(Image image);
    }
}
