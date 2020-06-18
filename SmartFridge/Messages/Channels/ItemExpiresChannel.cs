using SmartFridge.ContentNS;
using SmartFridge.Messages.Message;
using System;
using System.Collections.Generic;

namespace SmartFridge.Messages.Channels
{
    class ItemExpiresChannel : IChannel
    {
        public event Action<IMessage> Send;

        public MessageList Messages { get; set; }
        public string Type { get { return "ItemExpires"; } }

        public ItemExpiresChannel(IEnumerable<Item> items, MessageList msgs)
        {
            m_items = items;
            m_msgs = msgs;

            Update();

            //TODO: Timer für tägliches Update
        }

        public void Update()
        {
            foreach(Item item in m_items)
            {
                // Warnung 20% oder min. 1 Tag vor Ablaufdatum
                int warningDurabilityInDays = (int)(item.Product.Durability * 0.2);
                if (warningDurabilityInDays == 0) warningDurabilityInDays = 1;

                if (item.Durability <= warningDurabilityInDays && item.Durability > 0) 
                    ItemExpires(item);

                if (item.Durability <= 0) 
                    ItemExpired(item);
            }
        }

        private void ItemExpires(Item item)
        {
            var msg = new ItemExpiresMessage(item);

            if (!m_msgs.Contains(msg)) {                
                m_msgs.Add(msg);
                Send?.Invoke(msg);
            }
        }

        private void ItemExpired(Item item)
        {
            var msg = new ItemExpiredMessage(item);
            var expiringMsg = new ItemExpiresMessage(item);

            if (!m_msgs.Contains(msg)) {
                m_msgs.Remove(expiringMsg);
                m_msgs.Add(msg);                
                Send?.Invoke(msg);
            }
        }

        private MessageList m_msgs;
        private IEnumerable<Item> m_items;      
    }
}
