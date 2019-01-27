using System;
using MixinSdk;

namespace mixin_sdk_test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("======== Mixin C# SDK Test ========= \n");
            MixinNetworkApi mixinNetworkApi = new MixinNetworkApi();
            mixinNetworkApi.Init(USRCONFIG.ClientId, USRCONFIG.ClientSecret, USRCONFIG.SessionId, USRCONFIG.PinToken, USRCONFIG.PrivateKey);

            MixinMessagerApi mixinMessagerApi = new MixinMessagerApi();
            mixinMessagerApi.Init(USRCONFIG.ClientId, USRCONFIG.ClientSecret, USRCONFIG.SessionId, USRCONFIG.PinToken, USRCONFIG.PrivateKey);


            Console.WriteLine("======== Initiation Finished ========= \n\n\n");

            Console.WriteLine("======== Test Create PIN ===========\n");

            //Console.WriteLine(mixinNetworkApi.CreatePIN(USRCONFIG.PinCode, "123456").ToString());

            Console.WriteLine();

            //Console.WriteLine(mixinNetworkApi.CreatePIN("123456", USRCONFIG.PinCode).ToString());

            Console.WriteLine("\n\n======== Test Verify PIN ===========\n");

            //Console.WriteLine(mixinNetworkApi.VerifyPIN(USRCONFIG.PinCode).ToString());


            Console.WriteLine("\n\n======== Test Deposit ===========\n");
            //Console.WriteLine(mixinNetworkApi.Deposit("3596ab64-a575-39ad-964e-43b37f44e8cb").ToString());


            Console.WriteLine("\n\n======== Test Read Assets ===========\n");

            //var assets = mixinNetworkApi.ReadAssets();
            //foreach (var asset in assets)
            //{
            //    Console.WriteLine(asset.ToString());
            //    Console.WriteLine();
            //}

            Console.WriteLine("\n\n======== Test Read Asset ===========\n");
            //Console.WriteLine(mixinNetworkApi.ReadAsset("6cfe566e-4aad-470b-8c9a-2fd35b49c68d"));

            Console.WriteLine("\n\n======== Test Search Assest ===========\n");

            //var assets = mixinNetworkApi.SearchAssets("CNB");
            //foreach (var asset in assets)
            //{
            //    Console.WriteLine(asset.ToString());
            //    Console.WriteLine();
            //}

            Console.WriteLine("\n\n======== Test Network Asset ===========\n");
            //Console.WriteLine(mixinNetworkApi.NetworkAsset("965e5c6e-434c-3fa9-b780-c50f43cd955c"));

            Console.WriteLine("\n\n======== Test Top Assets ===========\n");
            //var topasset = mixinNetworkApi.TopAssets();
            //foreach (var asset in topasset)
            //{
            //    Console.WriteLine(asset.ToString());
            //    Console.WriteLine();
            //}

            Console.WriteLine("\n\n======== Test Network Snapshot ===========\n");

            //Console.WriteLine(mixinNetworkApi.NetworkSnapshot("85b8c435-1ef6-4f2a-85f9-8def2a852473", false));
            //Console.WriteLine(mixinNetworkApi.NetworkSnapshot("85b8c435-1ef6-4f2a-85f9-8def2a852473", true));

            //Console.WriteLine("\n\n======== Test Network Snapshots\n ===========\n");

            //var snaps = mixinNetworkApi.NetworkSnapshots(2, "2019-01-02T15:04:05.999999999-07:00", null, null, false);
            //foreach (var sn in snaps)
            //{
            //    Console.WriteLine(sn.ToString());
            //    Console.WriteLine();
            //}

            //snaps = mixinNetworkApi.NetworkSnapshots(2, "2019-01-02T15:04:05.999999999-07:00", null, null, true);
            //foreach (var sn in snaps)
            //{
            //    Console.WriteLine(sn.ToString());
            //    Console.WriteLine();
            //}

            Console.WriteLine("\n\n======== External Transactions ===========\n");
            //var ts = mixinNetworkApi.ExternalTransactions("6cfe566e-4aad-470b-8c9a-2fd35b49c68d", null, null, null, 10, null);
            //foreach (var t in ts)
            //{
            //    Console.WriteLine(t.ToString());
            //    Console.WriteLine();
            //}

            //Console.WriteLine("\n\n======== Test Create Address ===========\n");
            //var addr = mixinNetworkApi.CreateAddress("965e5c6e-434c-3fa9-b780-c50f43cd955c", "0x078C5AF6C8Ab533b8ef7FAb822B5B5f70A9d1c35", "CNB withdraw123", null, null, USRCONFIG.PinCode);
            //Console.WriteLine(addr);

            //Console.WriteLine("\n\n======== Test Read Address ===========\n");
            //Console.WriteLine(mixinNetworkApi.ReadAddress(addr.address_id));

            //Console.WriteLine("\n\n======== Test Delete Address ===========\n");
            //Console.WriteLine(mixinNetworkApi.DeleteAddress(USRCONFIG.PinCode, addr.address_id));

            Console.WriteLine("\n\n======== Test Withdrawal Addresses ===========\n");
            //var addrlist = mixinNetworkApi.WithdrawalAddresses("965e5c6e-434c-3fa9-b780-c50f43cd955c");
            //foreach (var a in addrlist)
            //{
            //    Console.WriteLine(a.ToString());
            //    Console.WriteLine();
            //}

            Console.WriteLine("\n\n======== Test Withdrawal  ===========\n");
            //Console.WriteLine(mixinNetworkApi.Withdrawal(addrlist[0].address_id, "100", USRCONFIG.PinCode, System.Guid.NewGuid().ToString(), "Test withdraw"));


            Console.WriteLine("\n\n======== Test Search User  ===========\n");
            Console.WriteLine(mixinMessagerApi.SearchUser("31243"));

            Console.WriteLine("\n\n======== Test Read User  ===========\n");
            Console.WriteLine(mixinMessagerApi.ReadUser("cf0b9c0e-a80a-4044-a6dc-797400c148d7"));

            Console.WriteLine("\n\n======== Test Transfer  ===========\n");
            //Console.WriteLine(mixinNetworkApi.Transfer("965e5c6e-434c-3fa9-b780-c50f43cd955c", "cf0b9c0e-a80a-4044-a6dc-797400c148d7", "100", USRCONFIG.PinCode, System.Guid.NewGuid().ToString(), "Test Transfer"));


            Console.WriteLine("\n\n======== Test Verify Payment ===========\n");
            Console.WriteLine(mixinNetworkApi.VerifyPayment("965e5c6e-434c-3fa9-b780-c50f43cd955c", "cf0b9c0e-a80a-4044-a6dc-797400c148d7", "100", "414c95cc-d695-48df-b23b-00815b21ed00"));

            Console.WriteLine("\n\n======== Test Read Transfer ===========\n");
            Console.WriteLine(mixinNetworkApi.ReadTransfer("414c95cc-d695-48df-b23b-00815b21ed00"));

            Console.WriteLine("\n\n======== Test APP User ===========\n");
            //var u = mixinNetworkApi.APPUser("yongyejunwangTest", "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCswepxpghdzL6eSm8mGG/lS6NJ\nNfeEFcJTkwma4nTT3eP9PwMOIoIQ+dHwxWQM5un3qlUfFTafntApnaqwnb34/TrH\nsQxSYWu+vkKAT2reDHMAt6Os8yN3NlfTZNycDQ/qZof39I/Dcur9id2IQ4wRVcFk\nfqAARRp9OlDVg9MCUQIDAQAB");
            //Console.WriteLine(u);


        }
    }
}
