using System;
using Newtonsoft.Json;
namespace MixinSdk.Bean
{
    public class CreateAddressReq
    {
        public string asset_id { get; set; }
        public string label { get; set; }
        public string destination { get; set; }
        public string tag { get; set; }
        public string pin { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class Address
    {
        public string type { get; set; }
        public string address_id { get; set; }
        public string asset_id { get; set; }
        public string destination { get; set; }
        public string tag { get; set; }
        public string label { get; set; }
        public string fee_asset_id { get; set; }
        public string fee { get; set; }
        public string reserve { get; set; }
        public string dust { get; set; }
        public DateTime updated_at { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
