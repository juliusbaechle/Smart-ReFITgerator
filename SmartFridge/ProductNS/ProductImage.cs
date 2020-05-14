using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SmartFridge.ProductNS
{
    public class ProductImage : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ProductImage() { }

        public ProductImage(ProductImage copy)
        {
            if(copy.Bitmap != null) {
                Bitmap = copy.Bitmap.Clone();
            }            
            ID = copy.ID;
        }

        public void Load(IImageRepository imageRepository)
        {
            if (imageRepository.Contains(ID))
            {
                Bitmap = imageRepository.Load(ID);
            }
            else Bitmap = null;
        }

        public void Save(IImageRepository imageRepository)
        {
            string newId = imageRepository.Save(Bitmap);
            imageRepository.Delete(ID);
            ID = newId;
        }

        public void Delete(IImageRepository imageRepository)
        {
            imageRepository.Delete(ID);
        }

        public void Set(string path)
        {
            var ImageExtensions = new List<string> { ".JPG", ".JPE", ".BMP", ".GIF", ".PNG" };
            if (!ImageExtensions.Contains(Path.GetExtension(path).ToUpperInvariant())) return;

            var uri = new Uri(path, UriKind.Absolute);
            var image = new BitmapImage(uri);
            Bitmap = Resize(Crop(image));
        }

        private BitmapSource Crop(BitmapSource image)
        {
            Int32Rect rect;
            if (image.PixelHeight >= image.PixelWidth) {
                rect = new Int32Rect(0, (image.PixelHeight - image.PixelWidth) / 2, image.PixelWidth, image.PixelWidth);
            } else {
                rect = new Int32Rect((image.PixelWidth - image.PixelHeight) / 2, 0, image.PixelHeight, image.PixelHeight);
            }

            var croppedImage = new CroppedBitmap(image, rect);
            return croppedImage;
        }

        private BitmapSource Resize(BitmapSource image)
        {
            if (image.PixelHeight < 500) return image;

            double factor = 500.0 / image.PixelWidth;
            ScaleTransform scale = new ScaleTransform(factor, factor);
            return new TransformedBitmap(image, scale);
        }


        internal string ID { get; set; }

        public BitmapSource Bitmap
        {
            private set
            {
                if(m_bitmap != value) {
                    m_bitmap = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Bitmap"));
                }               
            }
            get { return m_bitmap; }
        }
        private BitmapSource m_bitmap = null;
    }
}
