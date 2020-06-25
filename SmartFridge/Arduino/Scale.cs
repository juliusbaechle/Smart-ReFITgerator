using System;

namespace SmartFridge.Arduino
{
    class Scale : IScale
    {
        public ulong GetWeightInGrams()
        {
            throw new NotImplementedException();
        }

        public bool Connected()
        {
            throw new NotImplementedException();
        }
    }
}
