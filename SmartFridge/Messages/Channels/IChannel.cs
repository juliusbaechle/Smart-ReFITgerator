using System;
using System.Collections.Generic;

namespace SmartFridge.Messages.Channels
{
    interface IChannel
    {
        event Action<IMessage> Send;

        string Type { get; }

        MessageList Messages { get; set; }
    }
}
