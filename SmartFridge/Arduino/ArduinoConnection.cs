using System;
using System.IO.Ports;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.Management;
using System.Linq;
using System.Windows;

namespace SmartFridge.Arduino
{
    class ArduinoConnection
    {
        public event Action<string> Received;
        public event Action<State> StateChanged;

        public enum State
        {
            None,
            Connected,
            Disconnected,
            Starting
        }

        public enum Command
        {            
            GetDeviceName = '1',
            GetWeight     = '2'
        }

        public State ConnectionState
        {
            get { return m_connectionState; }
            set
            {
                if (m_connectionState == value) return;
                m_connectionState = value;
                StateChanged?.Invoke(value);
            }
        }
        private State m_connectionState = State.None;



        public void Connect()
        {
            if (m_port == null)
            {
                m_port = new SerialPort();
                m_port.BaudRate = 9600;
                m_port.DtrEnable = true;
            }

            Task.Run(() => {
                List<string> unoPorts = GetUnoPorts();
                
                if(unoPorts.Count > 0) {
                    foreach (string port in unoPorts)
                        if (TryConnect(port)) break;
                }
                else {
                    foreach (string port in SerialPort.GetPortNames())
                        if (TryConnect(port)) break;
                }
            });
        }


        public List<string> GetUnoPorts()
        {
            List<string> unoPorts = new List<string>();

            using (var searcher = new ManagementObjectSearcher("SELECT * FROM WIN32_SerialPort"))
            {
                string[] portnames = SerialPort.GetPortNames();
                var ports = searcher.Get().Cast<ManagementBaseObject>().ToList();

                foreach (ManagementBaseObject port in ports)
                {
                    string description = port.Properties["Description"].Value.ToString();
                    if (description.Contains("Arduino Uno"))
                        unoPorts.Add(port.Properties["DeviceID"].Value.ToString());
                }
            }

            return unoPorts;
        }



        private bool TryConnect(string port)
        {
            try
            {
                m_port.PortName = port;
                m_port.Open();
                ConnectionState = State.Starting;

                Thread.Sleep(3500);
                
                m_port.ReadTimeout = 1000;
                m_port.WriteTimeout = 200;
                Transmit(Command.GetDeviceName);
                DeviceName = m_port.ReadLine();
                if (!DeviceName.Contains("smart refitgerator arduino")) return false;

                m_port.ReadTimeout = -1;
                ConnectionState = State.Connected;
                m_thread = new Thread(Communicate) { IsBackground = true };
                m_thread.Start();
                return true;
            }
            catch (Exception)
            {
                Disconnect();
                return false;
            }
        }

        private void Communicate()
        {
            try
            {
                while (true)
                {
                    string rxData = m_port.ReadLine();
                    Received?.Invoke(rxData);
                }
            }
            catch (System.IO.IOException)
            {
                Disconnect();
            }
        }

        public void Disconnect()
        {
            if (ConnectionState == State.Disconnected)
                return;

            m_port.Close();
            ConnectionState = State.Disconnected;
            DeviceName = "";
        }

        public void Transmit(Command cmd)
        {
            try 
            { 
            byte[] command = new byte[] { (byte)cmd };
            m_port.Write(command, 0, 1);
            }
            catch (TimeoutException)
            {
                Console.WriteLine("Error when sending to " + m_port.PortName);
            }
        }

        public string DeviceName { get; private set; }

        private SerialPort m_port;
        private Thread m_thread;        
    }
}
