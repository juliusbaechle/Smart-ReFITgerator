using System.Windows.Media.Imaging;

namespace SmartFridge.Messages
{
    public interface IMessage
    {
        string Title { get; }
        string Text { get; }
        BitmapSource Image { get; }
    }
}
