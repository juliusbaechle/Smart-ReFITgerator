using System;

namespace SmartFridge.Messages
{
    class Message
    {
        public Message(string title, string text)
        {
            Title = title;
            Text = text;
        }

        public string Title { get; set; }
        public string Text { get; set; }
    }

    class NotDeliveredException : Exception {};

    interface IMessenger
    {
        /// <exception cref="NotDeliveredException"></exception>
        bool Send(Message msg);

        string ConnectionData { get; set; }
    }
}
