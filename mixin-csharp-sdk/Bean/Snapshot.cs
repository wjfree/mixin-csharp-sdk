using System;
using Newtonsoft.Json;

namespace MixinSdk.Bean
{
    public class Snapshot
    {
        public string amount { get; set; }
        public Asset asset { get; set; }
        public string created_at { get; set; }
        public string data { get; set; }
        public string snapshot_id { get; set; }
        public string source { get; set; }
        public string type { get; set; }
        public string user_id { get; set; }
        public string trace_id { get; set; }
        public string opponent_id { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
