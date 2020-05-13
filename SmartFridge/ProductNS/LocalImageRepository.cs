using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
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

        public void Delete(string id)
        {
            if (File.Exists(GetPath(id)))
            {
                File.Delete(GetPath(id));
            }
        }

        public BitmapImage Load(string id)
        {
            if(!File.Exists(GetPath(id)))
                return new BitmapImage();

            Uri uri = new Uri(GetPath(id), UriKind.Relative);
            BitmapImage img = new BitmapImage(uri);
            return img;
        }

        public string Save(BitmapImage bitmap)
        {
            string id;
            do {
                id = RandomString(8);
            } while (File.Exists(GetPath(id)));

            PngBitmapEncoder encoder = new PngBitmapEncoder();
            string path = GetPath(id);
            encoder.Frames.Add(BitmapFrame.Create(bitmap));
            using (var filestream = new FileStream(path, FileMode.Create))
                encoder.Save(filestream);

            return id;
        }


        private string GetPath(string id)
        {
            return PATH + "/" + id + ".png";
        }
        
        public static string RandomString(int length)
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
