using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace SmartFridge.ProductNS
{
    class LocalImageRepository : ImageRepository
    {
        public LocalImageRepository(string path = DEFAULT_PATH)
        {
            m_path = path;

            if (!Directory.Exists(m_path))
                Directory.CreateDirectory(path);
        }

        public override bool Contains(ProductImage image)
        {
            return File.Exists(CreatePath(image.ID));
        }

        public override async Task LoadAsync(ProductImage image)
        {
            if (!Contains(image)) return;
            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.CacheOption = BitmapCacheOption.OnLoad;
            img.UriSource = new Uri(CreatePath(image.ID), UriKind.Relative);
            img.EndInit();
            image.Bitmap = img;
        }

        public override async Task DeleteAsync(ProductImage image)
        {
            if (Contains(image))
                File.Delete(CreatePath(image.ID));
        }

        internal override async Task SaveAsync(ProductImage image)
        {
            if (image.Bitmap == null) return;

            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image.Bitmap));

            using (var filestream = new FileStream(CreatePath(image.ID), FileMode.Create))
                encoder.Save(filestream);
        }


        private string CreatePath(string id)
        {
            return m_path + "/" + id + ".png";
        }

        private string m_path;

        #if DEBUG
            // Relativ zur Debug.exe
            private const string DEFAULT_PATH = "../../images";
        #else
            //Relativ zur Release.exe
            private const string DEFAULT_PATH = "images";
        #endif
    }
}
