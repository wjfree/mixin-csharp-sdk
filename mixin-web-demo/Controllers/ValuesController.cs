using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MixinSdk;
using mixin_web_demo;

namespace mixin_web_demo.Controllers
{
    [Route("auth")]
    public class AuthController : Controller
    {
        [HttpGet]
        public string Get([FromQuery]string code)
        {
            MixinApi mixinApi = new MixinApi();
            mixinApi.Init(USRCONFIG.ClientId, USRCONFIG.ClientSecret, USRCONFIG.SessionId, USRCONFIG.PinToken, USRCONFIG.PrivateKey);

            string token = mixinApi.GetClientAuthToken(code);
            System.Console.WriteLine(token);

            var a = mixinApi.ReadProfile(token);
            var b = mixinApi.ReadAssets(token);

            return b.ToString();
        }

    }
}
