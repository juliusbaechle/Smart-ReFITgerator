using System.Threading.Tasks;

namespace SmartFridge.Arduino
{
    public interface IScale
    {
        Task<ulong> GetWeightAsync();
        bool Connected();
        ulong Weight { get; set; }
    }
}
