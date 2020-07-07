using System.Diagnostics;
using System.Threading.Tasks;

namespace SmartFridge.Arduino
{
    class ScaleMoc : IScale
    {
        public ulong Weight { get; set; } = 250;

        public Task<ulong> GetWeightAsync()
        {
            return Task.Run(() => { return Weight; });
        }

        public bool Connected() { return true; }
    }
}
