using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MixinSdk.Bean
{
    public class ParticipantAction
    {
        public string action { get; set; }
        public string role { get; set; }
        public string user_id { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class CreateConversationReq
    {
        public string category { get; set; }
        public string conversation_id { get; set; }
        public List<ParticipantAction> participants { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class Participant
    {
        public string type { get; set; }
        public string user_id { get; set; }
        public string role { get; set; }
        public string created_at { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class Conversation
    {
        public string type { get; set; }
        public string conversation_id { get; set; }
        public string creator_id { get; set; }
        public string category { get; set; }
        public string name { get; set; }
        public string icon_url { get; set; }
        public string announcement { get; set; }
        public string created_at { get; set; }
        public string code_id { get; set; }
        public string code_url { get; set; }
        public string mute_until { get; set; }
        public List<Participant> participants { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
