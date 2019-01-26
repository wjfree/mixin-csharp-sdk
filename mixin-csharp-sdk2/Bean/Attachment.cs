using System;
using Newtonsoft.Json;
namespace MixinSdk.Bean
{
    public class Attachment
    {
        public string type { get; set; }
        public string attachment_id { get; set; }
        public string upload_url { get; set; }
        public string view_url { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
