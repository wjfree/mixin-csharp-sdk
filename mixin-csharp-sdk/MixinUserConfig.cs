using System;
using Newtonsoft.Json;
namespace MixinSdk
{
    public class MixinUserConfig
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string SessionId { get; set; }
        public string PinToken { get; set; }
        public string PrivateKey { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}

