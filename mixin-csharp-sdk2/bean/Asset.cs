using System;
using Newtonsoft.Json;

namespace MixinSdk.Bean
{
    public class Asset
    {
        public string type { get; set; }
        public string asset_id { get; set; }
        public string chain_id { get; set; }
        public string symbol { get; set; }
        public string name { get; set; }
        public string icon_url { get; set; }
        public string balance { get; set; }
        public string public_key { get; set; }
        public string account_name { get; set; }
        public string account_tag { get; set; }
        public string price_btc { get; set; }
        public string price_usd { get; set; }
        public string change_btc { get; set; }
        public string change_usd { get; set; }
        public string asset_key { get; set; }
        public int confirmations { get; set; }
        public double capitalization { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
