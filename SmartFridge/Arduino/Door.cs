using System;

namespace SmartFridge.Arduino
{
    public class Door : IDoor
    {
        public event Action Opened;
        public event Action Closed;
    }
}
