using System;
using System.Threading.Tasks;

namespace SmartFridge.Arduino
{
    class ArduinoDevice : IDoor, IScale
    {
        public event Action<bool> ConnectionChanged;
        public ArduinoConnection Connection { get; set; }

        public ArduinoDevice()
        {
            Connection = new ArduinoConnection();
            Connection.Received += Evaluate;
            Connection.StateChanged += OnConnectionStateChanged;
            Connection.Connect();
        }

        private void Evaluate(string rx)
        {
            if (rx.Contains("door opened")) Open = true;
            if (rx.Contains("door closed")) Open = false;
            if (rx.Contains("weight: ")) 
            {
                if (rx.Contains("-")) Weight=0;
                else Weight = UInt64.Parse(rx.Remove(0, 8));                
            };
        }


        public event Action<bool> DoorStateChanged;

        public bool Open
        {
            get { return m_doorOpen; }
            set
            {
                if (value == m_doorOpen) return;
                m_doorOpen = value;
                DoorStateChanged?.Invoke(value);
            }
        }
        private bool m_doorOpen = false;


        public Task<ulong> GetWeightAsync()
        {
            Connection.Transmit(ArduinoConnection.Command.GetWeight);
            waitForWeight= new TaskCompletionSource<ulong>();
            return waitForWeight.Task;
        }
        private TaskCompletionSource<ulong> waitForWeight = new TaskCompletionSource<ulong>();


        public ulong Weight
        {
            get { return m_weight; }
            set
            {   
                waitForWeight.SetResult(value);
                m_weight = value;                
            }
        }
        private ulong m_weight = UInt64.MaxValue;


        public bool Connected { get; private set; } = false;

        private void OnConnectionStateChanged(ArduinoConnection.State state)
        {
            bool newState = (state == ArduinoConnection.State.Connected);
            if (newState != Connected) ConnectionChanged?.Invoke(newState);
            Connected = newState;
        }
    }
}
