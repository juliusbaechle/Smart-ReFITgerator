using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

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
            var task = SendAsync(msg);
            task.Wait();
            return task.Result;
        }

        private async Task<bool> SendAsync(Message msg)
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
                HttpResponseMessage response = await client.PostAsync("https://rest.nexmo.com/sms/json", requestContent);
                string json = await response.Content.ReadAsStringAsync();
                return MessageSucceeded(json);
            }
            catch
            {
                return false;
            }
        }

        public static bool MessageSucceeded(string json)
        {
            JObject obj = JObject.Parse(json);
            var messages = obj.Value<JArray>("messages");
            var message = messages[0].ToObject<JObject>();
            var status = message.Value<int>("status");
            return status == 0;
        }

        public string ConnectionData { get; set; }
    }
}