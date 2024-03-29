# Mixin Csharp Sdk
[![Build status](https://ci.appveyor.com/api/projects/status/70obdywndwo5pvc6?svg=true)](https://ci.appveyor.com/project/wjfree/mixin-csharp-sdk)
[![nuget](https://img.shields.io/nuget/v/MixinCSharpSdk.svg)](https://www.nuget.org/packages/MixinCSharpSdk/)

## Introduction
Mixin-Csharp-Sdk is a cross-platform high performance SDK based on .Net Standard2.0 standard, which implements all the interfaces on https://developers.mixin.one/api. It has both synchronous and asynchronous interfaces, and can be used with .

Dependencies.
- RestSharp 106.15.0
- Newtonsoft.Json 13.0.1
- Portable.BouncyCastle 1.8.4
- SharpZipLib 1.4.1
- WebSocketProtocol 4.5.3

## API usage

```cs
MixinApi mixinApi = new MixinApi();
mixinApi.Init("client id", "client secret", "session id", "pin token", "private key");

mixinApi.CreatePIN("old pin", "123456");

mixinApi.VerifyPIN("pin");

mixinApi.Deposit("3596ab64-a575-39ad-964e-43b37f44e8cb");
var assets = mixinApi.ReadAssets();
mixinApi.ReadAsset("6cfe566e-4aad-470b-8c9a-2fd35b49c68d"));

assets = mixinApi.SearchAssets("CNB");
mixinApi.NetworkAsset("965e5c6e-434c-3fa9-b780-c50f43cd955c");

var topasset = mixinApi.TopAssets();

mixinApi.NetworkSnapshot("85b8c435-1ef6-4f2a-85f9-8def2a852473", false));

mixinApi.NetworkSnapshot("85b8c435-1ef6-4f2a-85f9-8def2a852473", true));

var snaps = mixinApi.NetworkSnapshots(2, "2019-01-02T15:04:05.999999999-07:00", null, null, false);

snaps = mixinApi.NetworkSnapshots(2, "2019-01-02T15:04:05.999999999-07:00", null, null, true);

var ts = mixinApi.ExternalTransactions("6cfe566e-4aad-470b-8c9a-2fd35b49c68d", null, null, null, 10, null);

var addr = mixinApi.CreateAddress("965e5c6e-434c-3fa9-b780-c50f43cd955c", "0xe6Bf2C2E8f3243dF46308ca472038eA9Fa1bc42C", "CNB withdraw", null, null, USRCONFIG.PinCode);
       
addr = mixinApi.CreateAddress("965e5c6e-434c-3fa9-b780-c50f43cd955c", "0x078C5AF6C8Ab533b8ef7FAb822B5B5f70A9d1c35", "CNB withdraw123", null, null, USRCONFIG.PinCode);
            Console.WriteLine(addr);

mixinApi.ReadAddress(addr.address_id);

mixinApi.DeleteAddress("pin", addr.address_id);

var addrlist = mixinApi.WithdrawalAddresses("965e5c6e-434c-3fa9-b780-c50f43cd955c");
 
mixinApi.Withdrawal(addrlist[0].address_id, "100", "pin", System.Guid.NewGuid().ToString(), "Test withdraw"));

var u = mixinApi.SearchUser("user id");

mixinApi.ReadUser("cf0b9c0e-a80a-4044-a6dc-797400c148d7");
            
mixinApi.Transfer("965e5c6e-434c-3fa9-b780-c50f43cd955c", "cf0b9c0e-a80a-4044-a6dc-797400c148d7", "100", "pin", System.Guid.NewGuid().ToString(), "Test Transfer");

mixinApi.VerifyPayment("965e5c6e-434c-3fa9-b780-c50f43cd955c", "cf0b9c0e-a80a-4044-a6dc-797400c148d7", "100", "414c95cc-d695-48df-b23b-00815b21ed00");

mixinApi.ReadTransfer("414c95cc-d695-48df-b23b-00815b21ed00"));


var user = mixinApi.APPUser("Csharp" + (new Random().Next() % 100) + "test", "private key");
 mixinApi.ReadProfile();
    
mixinApi.UpdateProfile("mixin_csharp_sdk_demo", "");

mixinApi.UpdatePerference("CONTACTS", "CONTACTS");
           
mixinApi.RotateUserQR();

var friends = mixinApi.Friends();

mixinApi.CreateAttachment();

var users = new List<ParticipantAction>();
users.Add(new ParticipantAction { action = "ADD", role = "", user_id = u.user_id });
mixinApi.CreateConversation("GROUP", users);

mixinApi.CreateConversation("CONTACT", users);

mixinApi.ReadConversation("fd72abcd-b080-3e0e-bfea-a0b1282b4bd0");

```
## WebSocket API
WebSocket is a full-duplex communication, so the API uses asynchronous calls to call the specific usage as follows.
```cs
await mixinApi.WebSocketConnect(HandleOnRecivedMessage, HandleOnOpened, HandleOnClosed);

await mixinApi.StartRecive();

await mixinApi.SendTextMessage("fd72abcd-b080-3e0e-bfea-a0b1282b4bd0", msg);

static void HandleOnRecivedMessage(object sender, EventArgs args, string message)
{
    System.Console.WriteLine(message);
}

static void HandleOnOpened(object sender, EventArgs args)
{
    System.Console.WriteLine("Opened~");
}
static void HandleOnClosed(object sender, EventArgs args)
{
    System.Console.WriteLine("Closed~");
}
```
## Reading other users' information
The following steps are required to read the user's personal information.
- Build an extranet web service for receiving callbacks from customers
- Configure the above callback address in the app's configuration center! [][1]
- Call the GetOAuthString method to get the authentication address and send it to the other party, or have the user click on it in your web page
- After the user confirms, the web service will receive a code string, call the GetClientAuthToken method to get the token of the user
- Pass in the token when calling the Api
	 
```cs
ReadProfile(token);
Deposit(token);
```

[1]:	https://i.loli.net/2019/02/07/5c5be23b2a5e5.png "配置图"
