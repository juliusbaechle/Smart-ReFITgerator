using SmartFridge.Arduino;
using SmartFridge.Messages.Message;
using System;
using System.Timers;

namespace SmartFridge.Messages.Channels
{
    public class FridgeOpenChannel : IChannel
    {
        public event Action<IMessage> Send;

        public FridgeOpenChannel(IDoor door)
        {
            door.DoorStateChanged += OnDoorStateChanged;
            door.ConnectionChanged += connected => { OnDoorStateChanged(false); };
            m_timer.Elapsed += OnTimeout;
        }

        private void OnDoorStateChanged(bool open)
        {
            if (open)
            {
                Messages?.Add(m_msg);
                m_timer.Start();
            }
            else
            {
                Messages?.Remove(m_msg);
                m_timer.Stop();
            }
        }

        private void OnTimeout(object o, ElapsedEventArgs e)
        {
            Send?.Invoke(new FridgeOpenMessage());            
        }        

        public string Type { get { return "FridgeOpen"; } }
        public MessageList Messages { get; set; }

        private Timer m_timer = new Timer(30 * 1000) { AutoReset = false };
        private IMessage m_msg = new FridgeOpenMessage();
    }
}
