using System;
using Newtonsoft.Json;

namespace MixinSdk.Bean
{
    public class Data
    {
        public string data { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
