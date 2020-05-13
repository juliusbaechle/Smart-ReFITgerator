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

        public BitmapImage Load(string path)
        {
            if(path == "" || path == null || !File.Exists(path))
                return new BitmapImage();
            
            BitmapImage img = new BitmapImage();
            img.BeginInit();
            img.CacheOption = BitmapCacheOption.OnLoad;
            img.UriSource = new Uri(path, UriKind.Relative);
            img.EndInit();

            return img;
        }

        public void Delete(string path)
        {
            if (File.Exists(path)) 
                File.Delete(path);
        }

        public string Save(BitmapImage image)
        {
            string path;
            do {
                path = CreatePath(RandomString(8));
            } while (File.Exists(path));

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image.UriSource));
            using (var filestream = new FileStream(path, FileMode.Create))
                encoder.Save(filestream);

            return path;
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
