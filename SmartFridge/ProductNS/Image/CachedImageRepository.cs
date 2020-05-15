using System;
using System.Windows.Media.Imaging;

namespace SmartFridge.ProductNS
{
    class CachedImageRepository : ImageRepository
    {
        public CachedImageRepository(ImageRepository cache, ImageRepository remote)
        {
            m_cache = cache;
            m_remote = remote;
        }

        public override bool Contains(string id)
        {
            if (m_cache.Contains(id))  return true;
            if (m_remote.Contains(id)) return true;
            return false;
        }

        public override void Delete(string id)
        {
            m_cache.Delete(id);
            m_remote.Delete(id);
        }

        public override BitmapSource Load(string id)
        {
            if (m_cache.Contains(id)) {
                return m_cache.Load(id);
            }
            
            BitmapSource image = m_remote.Load(id);
            m_cache.Save(image);
            return image;
        }

        internal override void Save(BitmapSource image, string id)
        {
            m_cache.Save(image, id);
            m_remote.Save(image, id);
        }

        ImageRepository m_cache;
        ImageRepository m_remote;
    }
}
