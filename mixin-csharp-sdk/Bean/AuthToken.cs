using System;
using Newtonsoft.Json;
namespace MixinSdk.Bean
{
    public class AuthTokenReq
    {
        public string client_id { get; set; }
        public string code { get; set; }
        public string client_secret { get; set; }
        public string code_verifier { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class AuthTokenRsp
    {
        public string access_token { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
