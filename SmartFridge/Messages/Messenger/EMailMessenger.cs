using System;
using System.Net;
using System.Net.Mail;

namespace SmartFridge.Messages
{
    class EMailMessenger : Messenger
    {
        public EMailMessenger(string emailAddress)
        {
            ConnectionData = emailAddress;
        }

        public bool Send(IMessage msg)
        {            
            string from = "smart.refitgerator@gmail.com";
            string to = ConnectionData;
            var message = new MailMessage(from, to);
            message.Subject = msg.Title;
            message.Body = msg.Text;

            var client = new SmtpClient("smtp.gmail.com", 587);
            client.Credentials = new NetworkCredential("smart.refitgerator@gmail.com", "refitgerator");
            client.EnableSsl = true;

            try
            {
                client.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public string ConnectionData { get; set; }
        public string Type { get { return "EMail"; } }
        public string ChannelID { get; set; }
    }
}
