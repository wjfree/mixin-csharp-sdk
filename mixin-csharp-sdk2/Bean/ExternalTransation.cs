using System;
using Newtonsoft.Json;
namespace MixinSdk.Bean
{
    public class ExternalTransation
    {
        public string type { get; set; }
        public string transaction_id { get; set; }
        public string transaction_hash { get; set; }
        public string sender { get; set; }
        public string amount { get; set; }
        public string public_key { get; set; }
        public string account_name { get; set; }
        public string account_tag { get; set; }
        public string asset_id { get; set; }
        public string chain_id { get; set; }
        public int confirmations { get; set; }
        public int threshold { get; set; }
        public string created_at { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
