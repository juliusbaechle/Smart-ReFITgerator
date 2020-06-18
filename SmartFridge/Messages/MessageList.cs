using System.Collections.Generic;
using System.ComponentModel;

namespace SmartFridge.Messages
{
    public class MessageList
    {
        public void Add(IMessage msg)
        {
            if (Contains(msg)) return;
            Msgs.Add(msg);
        }

        public void Remove(IMessage a_msg)
        {
            var copy = new List<IMessage>(Msgs);

            foreach (IMessage msg in copy)
                if (msg.Equals(a_msg)) 
                    Msgs.Remove(msg);
        }

        public bool Contains(IMessage a_msg)
        {
            foreach(IMessage msg in Msgs)
                if (msg.Equals(a_msg)) 
                    return true;

            return false;
        }

        public BindingList<IMessage> Msgs;
    }
}
