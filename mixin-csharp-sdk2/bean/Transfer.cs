using System;
using Newtonsoft.Json;

namespace MixinSdk.Bean
{
    public class TransferReq
    {
        public string amount { get; set; }
        public string asset_id { get; set; }
        public string opponent_id { get; set; }
        public string memo { get; set; }
        public string pin { get; set; }
        public string trace_id { get; set; }
    }

    public class Transfer
    {
        public string type { get; set; }
        public string snapshot_id { get; set; }
        public string opponent_id { get; set; }
        public string asset_id { get; set; }
        public string amount { get; set; }
        public string trace_id { get; set; }
        public string memo { get; set; }
        public string created_at { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
