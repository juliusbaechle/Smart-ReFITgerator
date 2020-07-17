using System.Windows.Media.Imaging;

namespace SmartFridge.Messages.Message
{
    class FridgeOpenMessage : IMessage
    {
        public string Title
        {
            get { return "Ihr Kühlschrank ist offen"; }
        }

        public string Text
        {
            get { return "Sie haben Ihre Kühlschranktür offen gelassen"; }
        }

        public BitmapSource Image {
            get { return m_image; }
            private set { m_image = value; }
        }
        private BitmapSource m_image = new BitmapImage();
    }
}
