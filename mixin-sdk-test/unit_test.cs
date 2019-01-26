using System;
using MixinSdk;

namespace mixin_sdk_test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("======== Mixin C# SDK Test ========= \n");
            Console.WriteLine("======== Test Verify PIN ===========\n");

            MixinNetworkApi mixinNetworkApi = new MixinNetworkApi();
            mixinNetworkApi.Init(USRCONFIG.ClientId, USRCONFIG.ClientSecret, USRCONFIG.SessionId, USRCONFIG.PinToken, USRCONFIG.PrivateKey);

            //object r = mixinNetworkApi.VerifyPIN("5851");
            object r = mixinNetworkApi.Deposit("3596ab64-a575-39ad-964e-43b37f44e8cb");
        }
    }
}
