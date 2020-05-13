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

        public string Save(BitmapImage image)
        {
            string path;
            do {
                path = CreatePath(RandomString(8));
            } while (File.Exists(path));

            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));
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
