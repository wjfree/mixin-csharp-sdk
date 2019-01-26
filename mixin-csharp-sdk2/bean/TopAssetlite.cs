using System;
using System.Collections.Generic;

namespace MixinSdk.Bean
{
    public class Assetlite
    {
        public string amount { get; set; }
        public string asset_id { get; set; }
        public string icon_url { get; set; }
        public string symbol { get; set; }
        public string type { get; set; }
    }

    public class TopAssetReq
    {
        public List<Assetlite> assets { get; set; }
    }

}
