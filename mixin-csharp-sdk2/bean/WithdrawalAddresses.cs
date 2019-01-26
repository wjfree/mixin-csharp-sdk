using System;
using System.Collections.Generic;

namespace MixinSdk.Bean
{
    public class WithdrawalAddresses
    {
        public class Datum
        {
            public string type { get; set; }
            public string address_id { get; set; }
            public string asset_id { get; set; }
            public string public_key { get; set; }
            public string label { get; set; }
            public string account_name { get; set; }
            public string account_tag { get; set; }
            public string fee { get; set; }
            public string reserve { get; set; }
            public string dust { get; set; }
            public DateTime updated_at { get; set; }
        }

        public class RootObject
        {
            public List<Datum> data { get; set; }
        }
    }
}
