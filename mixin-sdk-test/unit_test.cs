using System;
using System.Collections.Generic;
using MixinSdk;
using MixinSdk.Bean;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;

namespace mixin_sdk_test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("======== Mixin C# SDK Test ========= \n");
            MixinApi mixinApi = new MixinApi();
            mixinApi.Init(USRCONFIG.ClientId, USRCONFIG.ClientSecret, USRCONFIG.SessionId, USRCONFIG.PinToken, USRCONFIG.PrivateKey);

            Console.WriteLine("======== Initiation Finished ========= \n");

            Console.WriteLine("======== Test Create PIN ===========\n");

            Console.WriteLine(mixinApi.CreatePIN(USRCONFIG.PinCode, "123456").ToString());

            Console.WriteLine();

            Console.WriteLine(mixinApi.CreatePIN("123456", USRCONFIG.PinCode).ToString());

            Console.WriteLine("\n\n======== Test Verify PIN ===========\n");

            Console.WriteLine(mixinApi.VerifyPIN(USRCONFIG.PinCode).ToString());


            Console.WriteLine("\n\n======== Test Deposit ===========\n");
            Console.WriteLine(mixinApi.Deposit("3596ab64-a575-39ad-964e-43b37f44e8cb").ToString());


            Console.WriteLine("\n\n======== Test Read Assets ===========\n");

            var assets = mixinApi.ReadAssets();
            foreach (var asset in assets)
            {
                Console.WriteLine(asset.ToString());
                Console.WriteLine();
            }

            Console.WriteLine("\n\n======== Test Read Asset ===========\n");
            Console.WriteLine(mixinApi.ReadAsset("6cfe566e-4aad-470b-8c9a-2fd35b49c68d"));

            Console.WriteLine("\n\n======== Test Search Assest ===========\n");

            assets = mixinApi.SearchAssets("CNB");
            foreach (var asset in assets)
            {
                Console.WriteLine(asset.ToString());
                Console.WriteLine();
            }

            Console.WriteLine("\n\n======== Test Network Asset ===========\n");
            Console.WriteLine(mixinApi.NetworkAsset("965e5c6e-434c-3fa9-b780-c50f43cd955c"));

            Console.WriteLine("\n\n======== Test Top Assets ===========\n");
            var topasset = mixinApi.TopAssets();
            foreach (var asset in topasset)
            {
                Console.WriteLine(asset.ToString());
                Console.WriteLine();
            }

            Console.WriteLine("\n\n======== Test Network Snapshot don't auth ===========\n");
            Console.WriteLine(mixinApi.NetworkSnapshot("85b8c435-1ef6-4f2a-85f9-8def2a852473", false));


            Console.WriteLine("\n\n======== Test Network Snapshot auth ===========\n");
            Console.WriteLine(mixinApi.NetworkSnapshot("85b8c435-1ef6-4f2a-85f9-8def2a852473", true));

            Console.WriteLine("\n\n======== Test Network Snapshots ===========\n");

            var snaps = mixinApi.NetworkSnapshots(2, "2019-01-02T15:04:05.999999999-07:00", null, null, false);
            foreach (var sn in snaps)
            {
                Console.WriteLine(sn.ToString());
                Console.WriteLine();
            }

            snaps = mixinApi.NetworkSnapshots(2, "2019-01-02T15:04:05.999999999-07:00", null, null, true);
            foreach (var sn in snaps)
            {
                Console.WriteLine(sn.ToString());
                Console.WriteLine();
            }

            Console.WriteLine("\n\n======== External Transactions ===========\n");
            var ts = mixinApi.ExternalTransactions("6cfe566e-4aad-470b-8c9a-2fd35b49c68d", null, null, null, 10, null);
            foreach (var t in ts)
            {
                Console.WriteLine(t.ToString());
                Console.WriteLine();
            }

            Console.WriteLine("\n\n======== Test Create Address ===========\n");
            var addr = mixinApi.CreateAddress("965e5c6e-434c-3fa9-b780-c50f43cd955c", "0xe6Bf2C2E8f3243dF46308ca472038eA9Fa1bc42C", "CNB withdraw", null, null, USRCONFIG.PinCode);
            Console.WriteLine(addr);

            Console.WriteLine("\n\n======== Test Create Address ===========\n");
            addr = mixinApi.CreateAddress("965e5c6e-434c-3fa9-b780-c50f43cd955c", "0x078C5AF6C8Ab533b8ef7FAb822B5B5f70A9d1c35", "CNB withdraw123", null, null, USRCONFIG.PinCode);
            Console.WriteLine(addr);

            Console.WriteLine("\n\n======== Test Read Address ===========\n");
            Console.WriteLine(mixinApi.ReadAddress(addr.address_id));

            Console.WriteLine("\n\n======== Test Delete Address ===========\n");
            Console.WriteLine(mixinApi.DeleteAddress(USRCONFIG.PinCode, addr.address_id));

            Console.WriteLine("\n\n======== Test Withdrawal Addresses ===========\n");
            var addrlist = mixinApi.WithdrawalAddresses("965e5c6e-434c-3fa9-b780-c50f43cd955c");
            foreach (var a in addrlist)
            {
                Console.WriteLine(a.ToString());
                Console.WriteLine();
            }

            Console.WriteLine("\n\n======== Test Withdrawal  ===========\n");
            Console.WriteLine(mixinApi.Withdrawal(addrlist[0].address_id, "100", USRCONFIG.PinCode, System.Guid.NewGuid().ToString(), "Test withdraw"));


            Console.WriteLine("\n\n======== Test Search User  ===========\n");
            var u = mixinApi.SearchUser("31243");
            Console.WriteLine(u);

            Console.WriteLine("\n\n======== Test Read User  ===========\n");
            Console.WriteLine(mixinApi.ReadUser("cf0b9c0e-a80a-4044-a6dc-797400c148d7"));

            Console.WriteLine("\n\n======== Test Transfer  ===========\n");
            Console.WriteLine(mixinApi.Transfer("965e5c6e-434c-3fa9-b780-c50f43cd955c", "cf0b9c0e-a80a-4044-a6dc-797400c148d7", "100", USRCONFIG.PinCode, System.Guid.NewGuid().ToString(), "Test Transfer"));


            Console.WriteLine("\n\n======== Test Verify Payment ===========\n");
            Console.WriteLine(mixinApi.VerifyPayment("965e5c6e-434c-3fa9-b780-c50f43cd955c", "cf0b9c0e-a80a-4044-a6dc-797400c148d7", "100", "414c95cc-d695-48df-b23b-00815b21ed00"));

            Console.WriteLine("\n\n======== Test Read Transfer ===========\n");
            Console.WriteLine(mixinApi.ReadTransfer("414c95cc-d695-48df-b23b-00815b21ed00"));

            Console.WriteLine("\n\n======== Test APP User ===========\n");

            var kpgen = new RsaKeyPairGenerator();

            kpgen.Init(new KeyGenerationParameters(new SecureRandom(new CryptoApiRandomGenerator()), 1024));

            var keyPair = kpgen.GenerateKeyPair();
            AsymmetricKeyParameter privateKey = keyPair.Private;
            AsymmetricKeyParameter publicKey = keyPair.Public;

            SubjectPublicKeyInfo info = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(keyPair.Public);
            string pk = Convert.ToBase64String(info.GetDerEncoded());


            var user = mixinApi.APPUser("Csharp" + (new Random().Next() % 100) + "test", pk);
            Console.WriteLine(user);

            Console.WriteLine("\n\n======== Test Read Profile ===========\n");
            Console.WriteLine(mixinApi.ReadProfile());

            Console.WriteLine("\n\n======== Test Update Profile ===========\n");
            Console.WriteLine(mixinApi.UpdateProfile("mixin_csharp_sdk_demo", "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o7ztZZestYBZ+2t7WLWgKyfTZxFdTXg11CqcPZbnXZ57tTX8eRBW/41SyKbhBicWurz1ZVvPXeDi9/zHedZcQ+hs6PM8W+U15x3yDAjP8BxY27EfuO11APwvqpXlIigj7n0AAAAldEVYdGRhdGU6Y3JlYXRlADIwMTktMDEtMDlUMDI6MTI6MzkrMDg6MDClD0PgAAAAJXRFWHRkYXRlOm1vZGlmeQAyMDE5LTAxLTA5VDAyOjEyOjM5KzA4OjAw1FL7XAAAAEN0RVh0c29mdHdhcmUAL3Vzci9sb2NhbC9pbWFnZW1hZ2ljay9zaGFyZS9kb2MvSW1hZ2VNYWdpY2stNy8vaW5kZXguaHRtbL21eQoAAAAYdEVYdFRodW1iOjpEb2N1bWVudDo6UGFnZXMAMaf/uy8AAAAYdEVYdFRodW1iOjpJbWFnZTo6SGVpZ2h0ADEyOEN8QYAAAAAXdEVYdFRodW1iOjpJbWFnZTo6V2lkdGgAMTI40I0R3QAAABl0RVh0VGh1bWI6Ok1pbWV0eXBlAGltYWdlL3BuZz+yVk4AAAAXdEVYdFRodW1iOjpNVGltZQAxNTQ2OTcxMTU59OK0DQAAABJ0RVh0VGh1bWI6OlNpemUAMTYxODVCiLvlAQAAAGJ0RVh0VGh1bWI6OlVSSQBmaWxlOi8vL2hvbWUvd3d3cm9vdC9uZXdzaXRlL3d3dy5lYXN5aWNvbi5uZXQvY2RuLWltZy5lYXN5aWNvbi5jbi9maWxlcy8xMDgvMTA4NjU1NS5wbmcDuUg9AAAAAElFTkSuQmCC"));

            Console.WriteLine("\n\n======== Test Update Perference ===========\n");
            Console.WriteLine(mixinApi.UpdatePerference("CONTACTS", "CONTACTS"));
            Console.WriteLine(mixinApi.UpdatePerference("EVERYBODY", "EVERYBODY"));

            Console.WriteLine("\n\n======== Test Rotate User's QR ===========\n");
            Console.WriteLine(mixinApi.RotateUserQR());

            Console.WriteLine("\n\n======== Test Friends ===========\n");
            var friends = mixinApi.Friends();
            foreach (var f in friends)
            {
                Console.WriteLine(f);
                Console.WriteLine();
            }

            Console.WriteLine("\n\n======== Test Create Attachment ===========\n");
            Console.WriteLine(mixinApi.CreateAttachment());


            Console.WriteLine("\n\n======== Test Create Conversation ===========\n");

            var users = new List<ParticipantAction>();
            users.Add(new ParticipantAction { action = "ADD", role = "", user_id = u.user_id });
            Console.WriteLine(mixinApi.CreateConversation("GROUP", users));

            Console.WriteLine(mixinApi.CreateConversation("CONTACT", users));

            Console.WriteLine("\n\n======== Test Read Conversation ===========\n");
            Console.WriteLine(mixinApi.ReadConversation("fd72abcd-b080-3e0e-bfea-a0b1282b4bd0"));



            mixinApi.WebSocketConnect(HandleOnRecivedMessage).Wait();

            mixinApi.StartRecive();

            do
            {
                var msg = Console.ReadLine();
                mixinApi.SendTextMessage("fd72abcd-b080-3e0e-bfea-a0b1282b4bd0", msg).Wait();
                
            } while (true);

        }

        static void HandleOnRecivedMessage(object sender, EventArgs args, string message)
        {
            System.Console.WriteLine(message);
        }

    }
}
