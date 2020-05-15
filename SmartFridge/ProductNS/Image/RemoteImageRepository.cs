using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Media.Imaging;

namespace SmartFridge.ProductNS
{
    class RemoteImageRepository : ImageRepository
    {
        ~RemoteImageRepository()
        {
            m_ftpClient.Dispose();
        }

        public override bool Contains(string id)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(CreateAddress(id));
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

        public override void Delete(string id)
        {
            Thread thread = new Thread(() => {
                try
                {
                    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(CreateAddress(id));
                    request.Credentials = m_credentials;
                    request.Method = WebRequestMethods.Ftp.DeleteFile;
                    FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                    response.Close();
                }
                catch { } //Datei nicht gefunden
            });
            thread.Start();
        }

        public override BitmapSource Load(string id)
        {
            try
            {
                byte[] data = m_ftpClient.DownloadData(new Uri(CreateAddress(id)));
                return Convert(data);
            }
            catch
            {
                return null;
            }
        }

        public override void LoadAsync(string id)
        {
            m_ftpClient.DownloadDataAsync(new Uri(CreateAddress(id)));
            m_ftpClient.DownloadDataCompleted +=
                (object sender, DownloadDataCompletedEventArgs e) =>
                {
                    try
                    {
                        byte[] data = e.Result;
                        DownloadCompleted?.Invoke(Convert(data), id);
                    }
                    catch { } //Datei nicht gefunden / nicht verbunden
                };
        }

        internal override void Save(BitmapSource image, string id)
        {
            try
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));

                var ms = new MemoryStream();
                encoder.Save(ms);
                m_ftpClient.UploadDataAsync(new Uri(CreateAddress(id)), ms.ToArray());
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
