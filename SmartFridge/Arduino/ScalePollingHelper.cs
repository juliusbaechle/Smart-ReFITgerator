using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace SmartFridge.Arduino
{
    class ScalePollingHelper
    {
        public event Action<ulong> WeightChanged;

        private IScale m_scale;
        private ulong m_weight;

        private System.Timers.Timer m_timer = new System.Timers.Timer(INTERVAL);
        private const int INTERVAL = 1500;
        private Mutex m_mutex = new Mutex();

        public ScalePollingHelper(IScale scale)
        {
            m_scale = scale;
            m_timer.Elapsed += (object o, ElapsedEventArgs e) => { Weight(); };
            m_timer.Start();
            Task.Run(Weight);
        }

        private void Weight()
        {
            if (!m_mutex.WaitOne(100)) return;

            Task<ulong> task = m_scale.GetWeightAsync();
            task.Wait();
            ulong weigth = task.Result; 
            if (m_weight != weigth) {
                m_weight = weigth;
                Application.Current.Dispatcher.Invoke(() => { WeightChanged?.Invoke(m_weight); });
            }

            m_mutex.ReleaseMutex();
        }

        public ulong GetWeightInGrams()
        {
            return m_weight;
        }
    }
}
