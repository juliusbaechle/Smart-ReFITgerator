using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SmartFridge.ProductNS
{
    class LocalImageRepository : IImageRepository
    {
        public LocalImageRepository()
        {
            #if DEBUG
            #else
            #endif
        }

        public bool Contains(string id)
        {
            return File.Exists(CreatePath(id));
        }

        public BitmapSource Load(string id)
        {
            string path = CreatePath(id);
            if(!File.Exists(path))
                return new BitmapImage();
            
            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.CacheOption = BitmapCacheOption.OnLoad;
            img.UriSource = new Uri(path, UriKind.Relative);
            img.EndInit();

            return img;
        }

        public void Delete(string id)
        {
            string path = CreatePath(id);
            if (File.Exists(path)) 
                File.Delete(path);
        }

        public string Save(BitmapSource image)
        {
            if (image == null) return "";

            string id;
            string path;
            do {
                id = RandomString(8);
                path = CreatePath(id);
            } while (File.Exists(path));

            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));

            using (var filestream = new FileStream(CreatePath(id), FileMode.Create))
                encoder.Save(filestream);

            return id;
        }


        private string CreatePath(string id)
        {
            return PATH + "/" + id + ".png";
        }
        
        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private static Random random = new Random();

        #if DEBUG
            // Relativ zur Debug.exe
            private const string PATH = "../../images";
        #else
            //Relativ zur Release.exe
            private const string PATH = "images";
        #endif
    }
}
