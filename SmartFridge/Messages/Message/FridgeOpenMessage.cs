using System.Windows.Media.Imaging;

namespace SmartFridge.Messages.Message
{
    class FridgeOpenMessage : IMessage
    {
        public FridgeOpenMessage()
        {
            Image = new BitmapImage();
        }

        public string Title
        {
            get { return "Ihr Kühlschrank ist offen"; }
        }

        public string Text
        {
            get { return "Ihr Smart ReFITgerator ist seit ueber 30 Sekunden geoeffnet"; }
        }

        public BitmapSource Image {
            get { return m_image; }
            private set { m_image = value; }
        }
        BitmapSource m_image = null;
    }
}
