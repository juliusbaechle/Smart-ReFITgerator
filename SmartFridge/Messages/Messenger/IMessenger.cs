using System;

namespace SmartFridge.Messages
{
    class NotDeliveredException : Exception {};

    interface Messenger
    {
        bool Send(IMessage msg);
        string ConnectionData { get; set; }
        string Type { get; }
        string ChannelID { get; set; }
    }
}
