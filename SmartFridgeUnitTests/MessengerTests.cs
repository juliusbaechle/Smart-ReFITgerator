using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmartFridge.Messages;
using SmartFridge.Messages.Message;

namespace SmartFridgeUnitTests
{
    [TestClass]
    public class MessengerTests
    {
        [TestMethod]
        public void SendWhatsApp()
        {
            var msg = new FridgeOpenMessage();
            var messenger = new WhatsAppMessenger("4915902600345");
            Assert.IsTrue(messenger.Send(msg));
        }

        [TestMethod]
        public void SendSMS()
        {
            var msg = new FridgeOpenMessage();
            var messenger = new SMSMessenger("4915902600345");
            Assert.IsTrue(messenger.Send(msg));
        }

        [TestMethod]
        public void SendEMail()
        {
            var msg = new FridgeOpenMessage();
            var messenger = new EMailMessenger("julius.baechle@yahoo.de");
            Assert.IsTrue(messenger.Send(msg));
        }
    }
}
