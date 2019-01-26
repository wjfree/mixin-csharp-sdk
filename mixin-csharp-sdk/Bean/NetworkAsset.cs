using System;
using Newtonsoft.Json;

namespace MixinSdk.Bean
{
    public class NetworkAsset
    {
        public string amount { get; set; }
        public string asset_id { get; set; }
        public string chain_id { get; set; }
        public string icon_url { get; set; }
        public string name { get; set; }
        public int snapshots_count { get; set; }
        public string symbol { get; set; }
        public string type { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
