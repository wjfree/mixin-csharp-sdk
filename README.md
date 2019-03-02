# Mixin Csharp Sdk
[![Build status](https://ci.appveyor.com/api/projects/status/70obdywndwo5pvc6?svg=true)](https://ci.appveyor.com/project/wjfree/mixin-csharp-sdk)
[![nuget](https://img.shields.io/nuget/v/MixinCSharpSdk.svg)](https://www.nuget.org/packages/MixinCSharpSdk/)

## 简介
Mixin-Csharp-Sdk是基于.Net Standard2.0标准开发的跨平台高性能的SDK，实现了https://developers.mixin.one/api 上的全部接口。具备同步与异步接口，可使用于.net Framework/dotnet core程序集。

依赖项：
- RestSharp 106.6.7
- jose-jwt 2.4.0
- Newtonsoft.Json 12.0.1
- Portable.BouncyCastle 1.8.4
- SharpZipLib 1.1.0
- System.Net.WebSockets.WebSocketProtocol 4.5.3
## API的使用
```cs
MixinApi mixinApi = new MixinApi();
mixinApi.Init("client id", "client secret", "session id", "pin token", "private key");

//创建PIN
mixinApi.CreatePIN("old pin", "123456");

//验证PIN
mixinApi.VerifyPIN("pin");

//获取充值地址
mixinApi.Deposit("3596ab64-a575-39ad-964e-43b37f44e8cb");

//读取资产列表
var assets = mixinApi.ReadAssets();

//读取资产
mixinApi.ReadAsset("6cfe566e-4aad-470b-8c9a-2fd35b49c68d"));

//查找资产
assets = mixinApi.SearchAssets("CNB");

//网络资产
mixinApi.NetworkAsset("965e5c6e-434c-3fa9-b780-c50f43cd955c");

//Top资产
var topasset = mixinApi.TopAssets();

//NetworkSnapshot without Auth
mixinApi.NetworkSnapshot("85b8c435-1ef6-4f2a-85f9-8def2a852473", false));

//NetworkSnapshot with Auth
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

//创建对话
var users = new List<ParticipantAction>();
users.Add(new ParticipantAction { action = "ADD", role = "", user_id = u.user_id });
mixinApi.CreateConversation("GROUP", users);

mixinApi.CreateConversation("CONTACT", users);

mixinApi.ReadConversation("fd72abcd-b080-3e0e-bfea-a0b1282b4bd0");


```
## WebSocket API说明
WebSocket是全双工的通讯，因此在API使用了异步调用的方式来调用具体用法如下：
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
    System.Console.WriteLine("Opened~~");
}
static void HandleOnClosed(object sender, EventArgs args)
{
    System.Console.WriteLine("Closed~~");
}
```
## 读取其他用户信息
需要以下步骤来读取用户的个人信息：
- 搭建一个外网的web service，用于接收客户的回调信息
- 在APP的配置中心配置上述的回调地址![][1]
- 调用 GetOAuthString 方法获取认证地址，发送给对方，或在你的网页中让用户点击
- 用户确认后，web service会收到一串code，调用GetClientAuthToken方法可以获取到该用户的token
- 调用Api时传入token
	 
```cs
ReadProfile(token);
Deposit(token);
```


[1]:	https://i.loli.net/2019/02/07/5c5be23b2a5e5.png "配置图"
