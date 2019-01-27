using System;
using Newtonsoft.Json;

namespace MixinSdk.Bean
{
    public class VerifyPaymentReq
    {
        public string amount { get; set; }
        public string asset_id { get; set; }
        public string opponent_id { get; set; }
        public string trace_id { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class Recipient
    {
        public string type { get; set; }
        public string user_id { get; set; }
        public string identity_number { get; set; }
        public string full_name { get; set; }
        public string avatar_url { get; set; }
        public string relationship { get; set; }
        public DateTime mute_until { get; set; }
        public string created_at { get; set; }
        public bool is_verified { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class VerifyPaymentRsp
    {
        public Recipient recipient { get; set; }
        public Asset asset { get; set; }
        public string amount { get; set; }
        public string status { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

}
