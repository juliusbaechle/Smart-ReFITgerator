using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace SmartFridge.ProductNS
{
    class LocalImageRepository : ImageRepository
    {
        public LocalImageRepository()
        {
            #if DEBUG
            #else
            #endif
        }

        public override bool Contains(ProductImage image)
        {
            return File.Exists(CreatePath(image.ID));
        }

        private void Load(ProductImage image)
        {
            if (!Contains(image)) return;
            
            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.CacheOption = BitmapCacheOption.OnLoad;
            img.UriSource = new Uri(CreatePath(image.ID), UriKind.Relative);
            img.EndInit();
            image.Bitmap = img;
        }

        public override void LoadAsync(ProductImage image)
        {
            Load(image);
        }

        public override void Delete(ProductImage image)
        {
            if (Contains(image))
                File.Delete(CreatePath(image.ID));
        }

        public override void Save(ProductImage image)
        {
            if (image.Bitmap == null) return;

            image.ID = CreateId();
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image.Bitmap));

            using (var filestream = new FileStream(CreatePath(image.ID), FileMode.Create))
                encoder.Save(filestream);
        }


        private string CreatePath(string id)
        {
            return PATH + "/" + id + ".png";
        }

        #if DEBUG
            // Relativ zur Debug.exe
            private const string PATH = "../../images";
        #else
            //Relativ zur Release.exe
            private const string PATH = "images";
        #endif
    }
}
