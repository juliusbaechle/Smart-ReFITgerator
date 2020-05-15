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

        public override bool Contains(string id)
        {
            return File.Exists(CreatePath(id));
        }

        public override BitmapSource Load(string id)
        {
            if (!Contains(id)) return null;
            
            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.CacheOption = BitmapCacheOption.OnLoad;
            img.UriSource = new Uri(CreatePath(id), UriKind.Relative);
            img.EndInit();
            return img;
        }

        public override void LoadAsync(string id)
        {
            DownloadCompleted?.Invoke(Load(id), id);
        }

        public override void Delete(string id)
        {
            if (Contains(id))
                File.Delete(CreatePath(id));
        }

        internal override void Save(BitmapSource image, string id)
        {
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));

            using (var filestream = new FileStream(CreatePath(id), FileMode.Create))
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
