using SmartFridge.ContentNS;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace SmartFridge.Messages.Message
{
    class ItemExpiresMessage : IMessage
    {
        public ItemExpiresMessage(Item item)
        {
            Item = item;
        }

        public string Title
        {
            get { return $"{Item.Name} expires soon"; }
        }

        public string Text
        {
            get { return $"{Item.Name} expires in { Item.Durability } days"; }
        }

        public bool Equals(ItemExpiresMessage msg)
        {
            return msg.Item.ID == Item.ID;
        }

        public Item Item { get; set; }

        public BitmapSource Image {
            get { return Item.Image; }
        }
    }
}
