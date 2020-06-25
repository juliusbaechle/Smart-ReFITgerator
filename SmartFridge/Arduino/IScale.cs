namespace SmartFridge.Arduino
{
    public interface IScale 
    {
        ulong GetWeightInGrams();
        bool Connected();
    }
}
