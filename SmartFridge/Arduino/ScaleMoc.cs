namespace SmartFridge.Arduino
{
    class ScaleMoc : IScale
    {
        public ulong GetWeightInGrams()
        {
            return 250;
        }

        public bool Connected()
        {
            return false;
        }
    }
}
