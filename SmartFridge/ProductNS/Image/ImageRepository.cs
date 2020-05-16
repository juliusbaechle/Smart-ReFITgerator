using System;
using System.Windows.Media.Imaging;

namespace SmartFridge.ProductNS
{
    public abstract class ImageRepository
    {
        public abstract void Save(ProductImage image);
        public abstract void LoadAsync(ProductImage image);
        public abstract void Delete(ProductImage image);
        public abstract bool Contains(ProductImage image);

        protected string CreateId()
        {
            Guid guid = Guid.NewGuid();
            string id = guid.ToString("N");
            return id.ToUpper();
        }
    }
}
