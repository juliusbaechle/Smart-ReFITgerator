using System.Collections.Generic;
using System.IO;
using System.Net.Http;

namespace SmartFridge.Messages
{
    class SMSMessenger : IMessenger
    {
        public SMSMessenger(string connectionData)
        {
            // In DEMO only numbers on whitelist: http://dashboard.nexmo.com
            ConnectionData = connectionData;  
        }        

        public bool Send(Message msg)
        {
            var client = new HttpClient();

            var requestContent = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("api_key", "9f1ab4a7"),
                new KeyValuePair<string, string>("api_secret", "eYFtJwfSRE76zgd7"),
                new KeyValuePair<string, string>("to", ConnectionData),
                new KeyValuePair<string, string>("from", "SmartFIT"),
                new KeyValuePair<string, string>("text", msg.Text),
                new KeyValuePair<string, string>("title", msg.Title)
            });

            try
            {
                var task = client.PostAsync("https://rest.nexmo.com/sms/json", requestContent);
                task.Wait();
                HttpContent responseContent = task.Result.Content;

                using (var output = File.OpenWrite("file.dat")) {
                    responseContent.CopyToAsync(output);
                }

                return MessageDelivered(responseContent);
            }
            catch
            {
                return false;
            }
        }

        private bool MessageDelivered(HttpContent response)
        {
            return false;
        }

        public string ConnectionData { get; set; }
    }
}