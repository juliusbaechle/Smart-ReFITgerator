using SmartFridge.ProductNS;
using System;
using System.Collections.Generic;
using System.Timers;
using System.Windows;

namespace SmartFridge
{
    interface ISynchronizer
    {
        void Synchronize();
    }

    class Synchronizer : ISynchronizer
    {
        public Action<bool> ConnectionState;

        public List<ISynchronizer> m_synchronizers = new List<ISynchronizer>();


        private readonly Timer m_timer;
        private const double INTERVAL = 5000;
        private readonly System.Threading.Mutex m_mutex = new System.Threading.Mutex();
        
        private bool m_connectionState = false;        

        public Synchronizer()
        {
            m_timer = new Timer();
            m_timer.Interval = INTERVAL;
            m_timer.Elapsed += (object o, ElapsedEventArgs e) => { Synchronize(); };
            m_timer.Start();
        }

        public void Synchronize()
        {
            // Maximal ein Thread im nachfolgenden Code
            if (!m_mutex.WaitOne(100)) return;

            bool online = true;

            foreach (ISynchronizer synchronizer in m_synchronizers)
            {
                try
                {
                    synchronizer.Synchronize();
                }
                catch
                {
                    online = false;
                    break;
                }
            }

            if(m_connectionState != online)
                Application.Current.Dispatcher.Invoke(() => {
                    ConnectionState?.Invoke(online);
                });

            m_connectionState = online;

            m_mutex.ReleaseMutex();
        }

        public void Add(ISynchronizer synchronizer)
        {
            m_synchronizers.Add(synchronizer);
        }
    }
}
