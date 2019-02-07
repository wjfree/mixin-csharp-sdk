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
            var jss = new JsonSerializerSettings();
            jss.NullValueHandling = NullValueHandling.Ignore;
            return JsonConvert.SerializeObject(this, jss);
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
            var jss = new JsonSerializerSettings();
            jss.NullValueHandling = NullValueHandling.Ignore;
            return JsonConvert.SerializeObject(this, jss);
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

    /// <summary>
    /// Message attachment.
    /// Can Used for Image, Video, Data message
    /// </summary>
    public class MsgAttachment
    {
        public string attachment_id { get; set; }
        public string mime_type { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public long size { get; set; }
        public long duration { get; set; }
        public string thumbnail { get; set; }
        public string name { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class AppButton
    {
        public string label { get; set; }
        public string color { get; set; }
        public string action { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class AppCard
    {
        public string icon_url { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string action { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }


}
