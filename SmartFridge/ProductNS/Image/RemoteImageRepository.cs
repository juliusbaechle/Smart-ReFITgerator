using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Media.Imaging;

namespace SmartFridge.ProductNS
{
    class RemoteImageRepository : ImageRepository
    {
        internal Action<ProductImage> DownloadCompleted;

        ~RemoteImageRepository()
        {
            m_ftpClient.Dispose();
        }

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
            Thread thread = new Thread(() => {
                try
                {
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(CreateAddress(image.ID));
                    request.Credentials = m_credentials;
                    request.Method = WebRequestMethods.Ftp.DeleteFile;
                    FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                    response.Close();
                }
                catch { } //Datei nicht gefunden
            });
            thread.Start();
        }

        private void Load(ProductImage image)
        {
            try
            {
                byte[] data = m_ftpClient.DownloadData(new Uri(CreateAddress(image.ID)));
                image.Bitmap = Convert(data);
            }
            catch { }
        }

        public override void LoadAsync(ProductImage image)
        {
            m_ftpClient.DownloadDataAsync(new Uri(CreateAddress(image.ID)));
            m_ftpClient.DownloadDataCompleted +=
                (object sender, DownloadDataCompletedEventArgs e) =>
                {
                    try
                    {
                        byte[] data = e.Result;
                        image.Bitmap = Convert(data);
                        DownloadCompleted?.Invoke(image);
                    }
                    catch { } //Datei nicht gefunden / nicht verbunden
                };
        }

        public override void Save(ProductImage image)
        {
            try
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image.Bitmap));

                var ms = new MemoryStream();
                encoder.Save(ms);
                m_ftpClient.UploadDataAsync(new Uri(CreateAddress(image.ID)), ms.ToArray());
                ms.Dispose();
            }
            catch { } //Nicht verbunden
        }

        private BitmapSource Convert(byte[] data)
        {
            var stream = new MemoryStream(data);
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = stream;
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            bitmap.Freeze();
            return bitmap;
        }

        private string CreateAddress(string id)
        {
            var path = $"ftp://ftp.unaux.com/htdocs/{id}.png";
            return path;
        }

        static NetworkCredential m_credentials = new NetworkCredential("unaux_25782073", "2iiyamvfhula");
        WebClient m_ftpClient = new WebClient { Credentials = m_credentials };
    }
}
