using Org.BouncyCastle.Crypto.Modes.Gcm;
using System;
using System.Threading.Tasks;

namespace SmartFridge.Arduino
{
    class ArduinoDevice : IDoor, IScale
    {
        public ArduinoConnection Connection { get; set; }



        public ArduinoDevice()
        {
            Connection = new ArduinoConnection();
            Connection.Received += Evaluate;
            Connection.Connect();
        }

        private void Evaluate(string rx)
        {
            if (rx.Contains("door opened")) DoorOpen = true;
            if (rx.Contains("door closed")) DoorOpen = false;
            if (rx.Contains("weight: ")) 
            {
                if (rx.Contains("-")) Weight=0;
                else Weight = UInt64.Parse(rx.Remove(0, 8));                
            };
        }


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
                //if (m_weight == value) return;
                m_weight = value;
                
            }
        }
        private ulong m_weight;

        public bool Connected()
        {
            return Connection.State == ArduinoConnection.States.Connected;

        }
    }
}
