using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace SmartFridge.ProductNS
{
    public class Image : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        IImageRepository m_imageRepository;
        private BitmapImage m_image = null;

        internal string ID { get; set; }

        public BitmapImage Bitmap
        {
            private set
            {
                m_image = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Bitmap"));
            }
            get { return m_image; }
        }

        public void SetRepository(IImageRepository imageRepository)
        {
            m_imageRepository = imageRepository;
        }

        public void Load()
        {
            Bitmap = m_imageRepository.Load(ID);
        }

        public void Save()
        {
            string newId = m_imageRepository.Save(Bitmap);
            m_imageRepository.Delete(ID);
            ID = newId;
        }

        public void Delete()
        {
            m_imageRepository.Delete(ID);
        }

        public void Set(string path)
        {
            var ImageExtensions = new List<string> { ".JPG", ".JPE", ".BMP", ".GIF", ".PNG" };
            if (!ImageExtensions.Contains(Path.GetExtension(path).ToUpperInvariant())) return;

            var uri = new Uri(path, UriKind.Absolute);
            var image = new BitmapImage(uri);
            image = CropImage(image);
            Bitmap = image;
        }

        private BitmapImage CropImage(BitmapImage image)
        {
            Int32Rect rect;
            if (image.Height >= image.Width)
            {
                rect = new Int32Rect(0, (int)((image.Height - image.Width) / 2), (int)image.Width, (int)image.Width);
            }
            else
            {
                rect = new Int32Rect((int)((image.Width - image.Height) / 2), 0, (int)image.Height, (int)image.Height);
            }

            var croppedImage = new CroppedBitmap(image, rect);
            return ConvertToBitmapImage(croppedImage);
        }

        private BitmapImage ConvertToBitmapImage(BitmapSource source)
        {
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            MemoryStream memoryStream = new MemoryStream();
            BitmapImage bitmapImg = new BitmapImage();

            encoder.Frames.Add(BitmapFrame.Create(source));
            encoder.Save(memoryStream);

            memoryStream.Position = 0;
            bitmapImg.BeginInit();
            bitmapImg.StreamSource = new MemoryStream(memoryStream.ToArray());
            bitmapImg.EndInit();
            bitmapImg.Freeze();
            memoryStream.Close();

            return bitmapImg;
        }
    }
}
