using System;
using System.Threading.Tasks;

namespace SmartFridge.Arduino
{
    public interface IScale
    {
        event Action<bool> ConnectionChanged;
        bool Connected { get; }

        Task<ulong> GetWeightAsync();
        ulong Weight { get; set; }
    }
}
