using MySql.Data.MySqlClient.Memcached;
using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization.Formatters;
using System.Threading;
using System.Windows.Media.Imaging;

namespace SmartFridge.ProductNS
{
    class RemoteImageRepository : ImageRepository
    {
        internal Action<ProductImage> DownloadCompleted;

        public override bool Contains(ProductImage image)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(CreateAddress(image.ID));
            request.Credentials = m_credentials;
            request.Method = WebRequestMethods.Ftp.GetFileSize;

            try
            {
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                return true;
            } 
            catch(WebException)
            {
                return false;
            }
        }

        public override void Delete(ProductImage image)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(CreateAddress(image.ID));
            request.Credentials = m_credentials;
            request.Method = WebRequestMethods.Ftp.DeleteFile;
            request.GetResponseAsync();
        }

        public override void LoadAsync(ProductImage image)
        {
            var client = new WebClient { Credentials = m_credentials };
            client.DownloadDataAsync(new Uri(CreateAddress(image.ID)));

            client.DownloadDataCompleted +=
                (object sender, DownloadDataCompletedEventArgs e) =>
                {
                    try
                    {
                        byte[] data = e.Result;
                        image.Bitmap = Convert(data);
                        DownloadCompleted?.Invoke(image);
                        client.Dispose();
                    }
                    catch
                    {
                        Console.WriteLine("Was not able to download image");
                    }
                };
        }

        internal override void SaveFixedID(ProductImage image)
        {
            using (var ms = new MemoryStream())
            using (var client = new WebClient { Credentials = m_credentials })
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image.Bitmap));
                encoder.Save(ms);
                client.UploadDataAsync(new Uri(CreateAddress(image.ID)), ms.ToArray());
            }
        }

        private BitmapSource Convert(byte[] data)
        {
            using(var ms = new MemoryStream(data))
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = ms;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                bitmap.Freeze();
                return bitmap;
            }
        }

        private string CreateAddress(string id)
        {
            var path = $"ftp://ftp.unaux.com/htdocs/{id}.png";
            return path;
        }

        static NetworkCredential m_credentials = new NetworkCredential("unaux_25782073", "2iiyamvfhula");
    }
}
