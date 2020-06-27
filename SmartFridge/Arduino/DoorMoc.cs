using System;

namespace SmartFridge.Arduino
{
    class DoorMoc : IDoor
    {
        public event Action Opened;
        public event Action Closed;

        public DoorMoc()
        {
            // Opened?.Invoke();
        }
    }
}
