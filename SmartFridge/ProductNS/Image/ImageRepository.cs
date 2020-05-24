using System;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace SmartFridge.ProductNS
{
    public abstract class ImageRepository
    {
        internal abstract void Save(Image image);
        public abstract void Load(Image image);
        public abstract void Delete(Image image);
        public abstract bool Contains(Image image);
    }
}
