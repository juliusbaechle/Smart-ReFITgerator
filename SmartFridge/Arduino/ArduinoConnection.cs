using System;
using System.Collections.Generic;
using System.Linq;
using System.IO.Ports;
using System.Management;
using System.Threading.Tasks;
using System.Threading;

namespace SmartFridge.Arduino
{
    class ArduinoConnection
    {
        public event Action<ArduinoConnection, string> PropertyChanged;
        public event Action<string> Received;

        public enum States
        {
            Connected,
            Disconnected,
            Starting
        }

        public enum Command
        {            
            GetDeviceName = '1',
            GetWeight     = '2'
        }

        public States State
        {
            get { return m_connectionState; }
            set
            {
                m_connectionState = value;
                PropertyChanged?.Invoke(this, "ConnectionState");
            }
        }
        private States m_connectionState = States.Disconnected;



        public void Connect()
        {
            if (m_port == null)
            {
                m_port = new SerialPort();
                m_port.BaudRate = 9600;
                m_port.DtrEnable = true;
            }

            Task.Run(() => {
                foreach (string port in SerialPort.GetPortNames())
                    if (TryConnect(port)) break;
            });
        }




        private bool TryConnect(string port)
        {
            try
            {
                m_port.PortName = port;
                m_port.Open();
                State = States.Starting;

                Thread.Sleep(3500);
                
                m_port.ReadTimeout = 1000;
                m_port.WriteTimeout = 200;
                Transmit(Command.GetDeviceName);
                DeviceName = m_port.ReadLine();
                if (!DeviceName.Contains("smart refitgerator arduino")) return false;

                m_port.ReadTimeout = -1;
                State = States.Connected;
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
            finally
            {
                Disconnect();
            }
        }

        public void Disconnect()
        {
            if (State == States.Disconnected)
                return;

            m_port.Close();
            State = States.Disconnected;
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

        public string DeviceName
        {
            get { return m_deviceName; }
            set
            {
                m_deviceName = value;
                PropertyChanged?.Invoke(this, "DeviceName");
            }
        }
        private string m_deviceName;

        private SerialPort m_port;
        private Thread m_thread;        
    }
}
