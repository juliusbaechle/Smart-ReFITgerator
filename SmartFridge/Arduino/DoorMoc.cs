using System;

namespace SmartFridge.Arduino
{
    public class DoorMoc : IDoor
    {
        public event Action Opened;
        public event Action Closed;

        public bool DoorOpen
        {
            get { return m_doorOpen; }
            set
            {
                if (value != m_doorOpen)
                {
                    m_doorOpen = value;

                    if (value) Opened?.Invoke();
                    else Closed.Invoke();
                }
            }
        }
        private bool m_doorOpen = false;
    }
}
