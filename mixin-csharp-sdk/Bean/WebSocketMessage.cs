using System;
using Newtonsoft.Json;
namespace MixinSdk.Bean
{

    public class WebSocketMessage
    {
        public string id { get; set; }
        public string action { get; set; }
        public Params @params { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }


    public class Params
    {
        public string type { get; set; }
        public string representative_id { get; set; }
        public string quote_message_id { get; set; }
        public string conversation_id { get; set; }
        public string user_id { get; set; }
        public string message_id { get; set; }
        public string category { get; set; }
        public string data { get; set; }
        public string status { get; set; }
        public string source { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class RecvWebSocketMessage
    {
        public string id { get; set; }
        public string action { get; set; }
        public Params data { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
