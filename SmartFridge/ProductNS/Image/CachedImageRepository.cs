using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace SmartFridge.ProductNS
{
    class CachedImageRepository : ImageRepository
    {
        public CachedImageRepository(LocalImageRepository cache, RemoteImageRepository remote)
        {
            m_cache = cache;
            m_remote = remote;
        }

        public override bool Contains(ProductImage image)
        {
            if (m_cache.Contains(image))  return true;
            if (m_remote.Contains(image)) return true;
            return false;
        }

        public override async Task DeleteAsync(ProductImage image)
        {
            await m_cache.DeleteAsync(image);
            await m_remote.DeleteAsync(image);
        }

        public override async Task LoadAsync(ProductImage image)
        {
            if (m_cache.Contains(image)) {
                await m_cache.LoadAsync(image);
            }
            else
            {
                await m_remote.LoadAsync(image);
                await m_cache.SaveAsync(image);
            }
        }

        internal override async Task SaveAsync(ProductImage image)
        {
            await m_cache.SaveAsync(image);
            await m_remote.SaveAsync(image);
        }

        LocalImageRepository m_cache;
        RemoteImageRepository m_remote;
    }
}
