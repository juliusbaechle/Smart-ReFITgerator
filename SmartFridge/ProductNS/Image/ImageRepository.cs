using System;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace SmartFridge.ProductNS
{
    public abstract class ImageRepository
    {
        internal abstract Task SaveAsync(ProductImage image);
        public abstract Task LoadAsync(ProductImage image);
        public abstract Task DeleteAsync(ProductImage image);
        public abstract bool Contains(ProductImage image);
    }
}
