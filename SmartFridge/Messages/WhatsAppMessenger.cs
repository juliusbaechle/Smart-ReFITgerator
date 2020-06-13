

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SmartFridge.Messages
{
    class WhatsAppMessenger : IMessenger
    {
        public WhatsAppMessenger(string connectionData)
        {
            ConnectionData = connectionData;
        }

        public bool Send(Message msg)
        {
            var task = SendAsync(msg);
            task.Wait();
            return task.Result;
        }

        public async Task<bool> SendAsync(Message msg)
        {
            var requestJson = CreateJson(msg);
            var content = new StringContent(requestJson);
            content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            var client = new HttpClient();            

            try
            {
                HttpResponseMessage response = await client.PostAsync("https://messages-sandbox.nexmo.com/v0.1/messages", content);
                string responseJson = await response.Content.ReadAsStringAsync();
                return MessageSucceeded(responseJson);
            }
            catch
            {
                return false;
            }
        }

        private string CreateJson(Message msg)
        {
            JObject request = new JObject();
                JObject from = new JObject();
                    from.Add("type", "whatsapp");
                    from.Add("number", "14157386170");
                request.Add("from", from);


                JObject to = new JObject();
                    to.Add("type", "whatsapp");
                    to.Add("number", ConnectionData);
                request.Add("to", to);

                JObject message = new JObject();
                    JObject content = new JObject();
                        content.Add("type", "text");
                        content.Add("text", msg.Text);
                    message.Add("content", content);
                request.Add("message", message);
            return request.ToString();
        }

        private bool MessageSucceeded(string json)
        {
            var obj = JObject.Parse(json);
            JToken token;
            return obj.TryGetValue("message_uuid", out token);
        }

        public string ConnectionData { get; set; }
    }
}
