using System;
using System.IO;
using System.Net;
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
            if (!Contains(id)) return;

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(CreateAddress(id));
            request.Credentials = m_credentials;
            request.Method = WebRequestMethods.Ftp.DeleteFile;
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            response.Close();
        }

        public override BitmapSource Load(string id)
        {
            try
            {
                byte[] data = m_ftpClient.DownloadData(new Uri(CreateAddress(id)));
                var stream = new MemoryStream(data);

                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = stream;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                bitmap.Freeze();
                return bitmap;
            }
            catch
            {
                return null;
            }
        }

        internal override void Save(BitmapSource image, string id)
        {
            try
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(image));

                using (var ms = new MemoryStream())
                {
                    encoder.Save(ms);
                    m_ftpClient.UploadDataAsync(new Uri(CreateAddress(id)), ms.ToArray());
                }
            }
            catch
            {
            }
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
