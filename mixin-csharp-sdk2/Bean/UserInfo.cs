using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MixinSdk.Bean
{
    public class VerifyPinReq
    {
        public string pin { get; set; }
    }

    public class CreatePinReq
    {
        public string old_pin { get; set; }
        public string pin { get; set; }
    }

    public class App
    {
        public string type { get; set; }
        public string app_id { get; set; }
        public string app_number { get; set; }
        public string redirect_uri { get; set; }
        public string home_uri { get; set; }
        public string name { get; set; }
        public string icon_url { get; set; }
        public string description { get; set; }
        public List<string> capabilites { get; set; }
        public string app_secret { get; set; }
        public string creator_id { get; set; }
    }

    public class UserInfo
    {
        public string type { get; set; }
        public string user_id { get; set; }
        public string identity_number { get; set; }
        public string full_name { get; set; }
        public string avatar_url { get; set; }
        public string relationship { get; set; }
        public DateTime mute_until { get; set; }
        public string created_at { get; set; }
        public bool is_verified { get; set; }
        public App app { get; set; }
        public string session_id { get; set; }
        public string phone { get; set; }
        public string pin_token { get; set; }
        public string invitation_code { get; set; }
        public string code_id { get; set; }
        public string code_url { get; set; }
        public bool has_pin { get; set; }
        public string receive_message_source { get; set; }
        public string accept_conversation_source { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class UserPerference
    {
        public string receive_message_source { get; set; }
        public string accept_conversation_source { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class UserProfile
    {
        public string full_name { get; set; }
        public string avatar_base64 { get; set; }
    }

    public class NewUser
    {
        public string full_name { get; set; }
        public string session_secret { get; set; }
    }

}
