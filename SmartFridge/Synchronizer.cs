using SmartFridge.ProductNS;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace SmartFridge
{
    interface ISynchronizer
    {
        void Synchronize();
        void Reset();
    }

    class Synchronizer
    {
        public Action<bool> ConnectionState;

        private readonly Timer m_timer;
        private const double INTERVAL = 5000;
        private readonly System.Threading.Mutex m_mutex = new System.Threading.Mutex();
        
        private bool m_connectionState = false;
        private List<ISynchronizer> m_synchronizers = new List<ISynchronizer>();

        public Synchronizer()
        {
            m_timer = new Timer();
            m_timer.Interval = INTERVAL;
            m_timer.Elapsed += (object o, ElapsedEventArgs e) => { Synchronize(); };
            m_timer.Start();

            Task.Run(Synchronize);
        }

        public void Synchronize()
        {
            // Maximal ein Thread im nachfolgenden Code
            if (!m_mutex.WaitOne(100)) return;

            bool online = CheckForInternetConnection();

            foreach (ISynchronizer synchronizer in m_synchronizers) {
                if (online) {
                    try { synchronizer.Synchronize(); }
                    catch { online = false; }
                } else {
                    synchronizer.Reset();
                }
            }

            if (m_connectionState != online) {
                Application.Current.Dispatcher.Invoke(() => {
                    ConnectionState?.Invoke(online);
                });
                m_connectionState = online;
            }

            m_mutex.ReleaseMutex();
        }

        public static bool CheckForInternetConnection()
        {
            // Überprüft in Millisekunden ob Verbindung besteht

            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://google.com/generate_204"))
                    return true;
            }
            catch
            {
                return false;
            }
        }

        public void Add(ISynchronizer synchronizer) {
            m_synchronizers.Add(synchronizer);
        }
    }
}
