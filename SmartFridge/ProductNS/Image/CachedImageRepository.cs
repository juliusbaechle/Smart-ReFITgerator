using System;
using System.Windows.Media.Imaging;

namespace SmartFridge.ProductNS
{
    class CachedImageRepository : ImageRepository
    {
        public CachedImageRepository(LocalImageRepository cache, RemoteImageRepository remote)
        {
            m_cache = cache;
            m_remote = remote;

            remote.DownloadCompleted += 
                (ProductImage image) => { 
                    m_cache.Save(image);  
                };
        }

        public override bool Contains(ProductImage image)
        {
            if (m_cache.Contains(image))  return true;
            if (m_remote.Contains(image)) return true;
            return false;
        }

        public override void Delete(ProductImage image)
        {
            m_cache.Delete(image);
            m_remote.Delete(image);
        }

        public override void LoadAsync(ProductImage image)
        {
            if (m_cache.Contains(image)) {
                m_cache.LoadAsync(image);
            }
            else
            {
                m_remote.LoadAsync(image);
            }
        }

        public override void Save(ProductImage image)
        {
            if (image.Bitmap == null) return;

            m_cache.Save(image);
            m_remote.Save(image);
        }

        LocalImageRepository m_cache;
        RemoteImageRepository m_remote;
    }
}
