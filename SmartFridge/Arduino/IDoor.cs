using System;

namespace SmartFridge.Arduino
{
    public interface IDoor
    {
        event Action<bool> ConnectionChanged;
        bool Connected { get; }

        event Action<bool> DoorStateChanged;
        bool Open { get; }
    }
}
