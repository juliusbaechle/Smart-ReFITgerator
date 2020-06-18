using SmartFridge.ContentNS;
using System.Windows.Media.Imaging;

namespace SmartFridge.Messages.Message
{
    class ItemExpiredMessage : IMessage
    {
        public ItemExpiredMessage(Item item)
        {
            Item = item;
        }

        public string Title
        {
            get { return $"{Item.Name} expired"; }
        }

        public string Text
        {
            get
            {
                if (Item.Durability == 0)
                    return $"{Item.Name} expired today";
                else
                    return $"{Item.Name} expired since { Item.Durability } days";
            }
        }

        public bool Equals(ItemExpiredMessage msg)
        {
            return msg.Item.ID == Item.ID;
        }

        public Item Item { get; set; }

        public BitmapSource Image {
            get { return Item.Image; }
        }
    }
}
