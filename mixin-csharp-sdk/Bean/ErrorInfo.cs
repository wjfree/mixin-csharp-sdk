using System;
using Newtonsoft.Json;
namespace MixinSdk.Bean
{
    public class ErrorInfo
    {
        public int status { get; set; }
        public int code { get; set; }
        public string description { get; set; }
    }

    public class MixinError
    {
        public ErrorInfo error { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
