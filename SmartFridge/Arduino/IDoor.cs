using System;

namespace SmartFridge.Arduino
{
    public interface IDoor
    {
        event Action Opened;
        event Action Closed;
    }
}
