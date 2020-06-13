using System;
using Nexmo.Api;
using Nexmo.Api.Request;

/*
namespace SmartFridge.Messages
{
    class SMSMessenger : IMessenger
    {
        public SMSMessenger(string connectionData)
        {
            ConnectionData = connectionData;
        }        

        public bool Send(Message msg)
        {
            var creds = new Credentials { 
                ApiKey = "9f1ab4a7", 
                ApiSecret = "eYFtJwfSRE76zgd7" 
            };

            var sms = new SMS.SMSRequest {
                from = "SmartFIT",
                to = "4915902600345",
                text = msg.Text
            };
            
            try
            {
                SMS.Send(sms);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }

        public string ConnectionData { get; set; }
    }
}
*/