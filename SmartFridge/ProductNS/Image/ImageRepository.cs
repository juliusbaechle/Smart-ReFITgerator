using System;
using System.Windows.Media.Imaging;

namespace SmartFridge.ProductNS
{
    public abstract class ImageRepository
    {
        public string Save(BitmapSource image)
        {
            if (image == null) return "";

            string id = CreateId();
            Save(image, id);
            return id;
        }
        internal abstract void Save(BitmapSource image, string id);

        public abstract BitmapSource Load(string id);
        public abstract void Delete(string id);
        public abstract bool Contains(string id);

        private string CreateId()
        {
            Guid guid = Guid.NewGuid();
            string id = guid.ToString("N");
            return id.ToUpper();
        }
    }
}
