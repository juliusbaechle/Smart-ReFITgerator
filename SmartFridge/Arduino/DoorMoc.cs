using System;

namespace SmartFridge.Arduino
{
    public class DoorMoc : IDoor
    {
        public event Action<bool> ConnectionChanged;
        public event Action<bool> DoorStateChanged;

        public bool Connected { get { return true; } }

        public bool Open {
            get { return m_doorOpen; }
            set
            {
                if (value == m_doorOpen) return;
                m_doorOpen = value;
                DoorStateChanged?.Invoke(value);
            }
        }
        private bool m_doorOpen;        
    }
}
