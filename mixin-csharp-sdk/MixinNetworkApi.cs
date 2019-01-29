using System.Collections.Generic;
using MixinSdk.Bean;
using Newtonsoft.Json;
using RestSharp;

namespace MixinSdk
{
    public class MixinNetworkApi : MixinApi
    {
        /// <summary>
        /// Create or update user's pin code.
        /// </summary>
        /// <returns>User Info</returns>
        /// <param name="oldPin">Old pin or null</param>
        /// <param name="newPin">New pin.</param>
        public UserInfo CreatePIN(string oldPin, string newPin)
        {
            CheckAuth();

            const string req = "/pin/update";

            var request = new RestRequest(req, Method.POST);

            var oldPinBlock = "";
            if (!string.IsNullOrEmpty(oldPin))
            {
                oldPinBlock = MixinUtils.GenEncrypedPin(oldPin, userConfig.PinToken, userConfig.SessionId, rsaParameters);
            }
            var newPinBlock = MixinUtils.GenEncrypedPin(newPin, userConfig.PinToken, userConfig.SessionId, rsaParameters);

            var p = new CreatePinReq();
            p.old_pin = oldPinBlock;
            p.pin = newPinBlock;

            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(p);

            string token = MixinUtils.GenJwtAuthCode("POST", req, JsonConvert.SerializeObject(p), userConfig.ClientId, userConfig.SessionId, priKey);

            var jwtAuth = new RestSharp.Authenticators.JwtAuthenticator(token);
            jwtAuth.Authenticate(client, request);

            var response = client.Execute<Data>(request);

            if (null == response.Data.data)
            {
                var errorinfo = JsonConvert.DeserializeObject<MixinError>(response.Content);
                throw new MixinException(errorinfo);
            }

            var rz = JsonConvert.DeserializeObject<UserInfo>(response.Data.data);

            return rz;
        }

        /// <summary>
        /// Verifie pin.
        /// </summary>
        /// <returns>User Info</returns>
        /// <param name="pin">Pin.</param>
        public UserInfo VerifyPIN(string pin)
        {
            CheckAuth();

            const string req = "/pin/verify";

            var request = new RestRequest(req, Method.POST);

            var pinBlock = MixinUtils.GenEncrypedPin(pin, userConfig.PinToken, userConfig.SessionId, rsaParameters);

            VerifyPinReq p = new VerifyPinReq();
            p.pin = pinBlock;

            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(p);

            string token = MixinUtils.GenJwtAuthCode("POST", req, JsonConvert.SerializeObject(p), userConfig.ClientId, userConfig.SessionId, priKey);

            var jwtAuth = new RestSharp.Authenticators.JwtAuthenticator(token);
            jwtAuth.Authenticate(client, request);

            var response = client.Execute<Data>(request);

            if (null == response.Data.data)
            {
                var errorinfo = JsonConvert.DeserializeObject<MixinError>(response.Content);
                throw new MixinException(errorinfo);
            }

            var rz = JsonConvert.DeserializeObject<UserInfo>(response.Data.data);

            return rz;
        }

        /// <summary>
        /// Deposit the specified assetID.
        /// </summary>
        /// <returns>The deposit.</returns>
        /// <param name="assetID">Asset identifier.</param>
        public Asset Deposit(string assetID)
        {
            CheckAuth();

            const string req = "/assets/";

            var request = new RestRequest(req + assetID, Method.GET);

            request.AddHeader("Content-Type", "application/json");

            string token = MixinUtils.GenJwtAuthCode("GET", req + assetID, "", userConfig.ClientId, userConfig.SessionId, priKey);

            var jwtAuth = new RestSharp.Authenticators.JwtAuthenticator(token);
            jwtAuth.Authenticate(client, request);

            var response = client.Execute<Data>(request);

            if (null == response.Data.data)
            {
                var errorinfo = JsonConvert.DeserializeObject<MixinError>(response.Content);
                throw new MixinException(errorinfo);
            }

            var rz = JsonConvert.DeserializeObject<Asset>(response.Data.data);

            return rz;
        }

        /// <summary>
        /// Withdrawal the specified addressId, amount, pin, traceId and memo.
        /// </summary>
        /// <returns>The withdrawal.</returns>
        /// <param name="addressId">Address identifier.</param>
        /// <param name="amount">Amount.</param>
        /// <param name="pin">Pin.</param>
        /// <param name="traceId">Trace identifier.</param>
        /// <param name="memo">Memo.</param>
        public WithDrawalInfo Withdrawal(string addressId, string amount, string pin, string traceId, string memo)
        {
            CheckAuth();

            const string req = "/withdrawals";

            var request = new RestRequest(req, Method.POST);

            var pinBlock = MixinUtils.GenEncrypedPin(pin, userConfig.PinToken, userConfig.SessionId, rsaParameters);

            WithDrawalReq p = new WithDrawalReq
            {
                address_id = addressId,
                amount = amount,
                pin = pinBlock,
                trace_id = traceId,
                memo = memo
            };

            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(p);

            string token = MixinUtils.GenJwtAuthCode("POST", req, JsonConvert.SerializeObject(p), userConfig.ClientId, userConfig.SessionId, priKey);

            var jwtAuth = new RestSharp.Authenticators.JwtAuthenticator(token);
            jwtAuth.Authenticate(client, request);

            var response = client.Execute<Data>(request);

            if (null == response.Data.data)
            {
                var errorinfo = JsonConvert.DeserializeObject<MixinError>(response.Content);
                throw new MixinException(errorinfo);
            }

            var rz = JsonConvert.DeserializeObject<WithDrawalInfo>(response.Data.data);

            return rz;
        }

        /// <summary>
        /// Creates the address.
        /// </summary>
        /// <returns>The address.</returns>
        /// <param name="assetId">Asset identifier.</param>
        /// <param name="publicKey">Public key.</param>
        /// <param name="label">Label.</param>
        /// <param name="accountName">Account name.</param>
        /// <param name="accountTag">Account tag.</param>
        /// <param name="pin">Pin.</param>
        public Address CreateAddress(string assetId, string publicKey, string label, string accountName, string accountTag, string pin)
        {
            CheckAuth();

            const string req = "/addresses";

            var request = new RestRequest(req, Method.POST);

            var pinBlock = MixinUtils.GenEncrypedPin(pin, userConfig.PinToken, userConfig.SessionId, rsaParameters);

            CreateAddressReq p = new CreateAddressReq
            {
                asset_id = assetId,
                public_key = publicKey,
                label = label,
                account_name = accountName,
                account_tag = accountTag,
                pin = pinBlock
            };

            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(p);

            string token = MixinUtils.GenJwtAuthCode("POST", req, JsonConvert.SerializeObject(p), userConfig.ClientId, userConfig.SessionId, priKey);

            var jwtAuth = new RestSharp.Authenticators.JwtAuthenticator(token);
            jwtAuth.Authenticate(client, request);

            var response = client.Execute<Data>(request);

            if (null == response.Data.data)
            {
                var errorinfo = JsonConvert.DeserializeObject<MixinError>(response.Content);
                throw new MixinException(errorinfo);
            }

            var rz = JsonConvert.DeserializeObject<Address>(response.Data.data);

            return rz;
        }

        /// <summary>
        /// Deletes the address.
        /// </summary>
        /// <returns><c>true</c>, if address was deleted, <c>false</c> otherwise.</returns>
        /// <param name="pin">Pin.</param>
        /// <param name="addressId">Address identifier.</param>
        public bool DeleteAddress(string pin, string addressId)
        {
            CheckAuth();

            string req = "/addresses/" + addressId + "/delete";

            var request = new RestRequest(req, Method.POST);

            var pinBlock = MixinUtils.GenEncrypedPin(pin, userConfig.PinToken, userConfig.SessionId, rsaParameters);

            VerifyPinReq p = new VerifyPinReq();
            p.pin = pinBlock;

            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(p);

            string token = MixinUtils.GenJwtAuthCode("POST", req, JsonConvert.SerializeObject(p), userConfig.ClientId, userConfig.SessionId, priKey);

            var jwtAuth = new RestSharp.Authenticators.JwtAuthenticator(token);
            jwtAuth.Authenticate(client, request);

            var response = client.Execute<Data>(request);

            if (!response.Content.Equals("{}"))
            {
                var errorinfo = JsonConvert.DeserializeObject<MixinError>(response.Content);
                throw new MixinException(errorinfo);
            }

            return true;
        }

        /// <summary>
        /// Reads the address.
        /// </summary>
        /// <returns>The address.</returns>
        /// <param name="addressId">Address identifier.</param>
        public Address ReadAddress(string addressId)
        {
            CheckAuth();

            const string req = "/addresses/";

            var request = new RestRequest(req + addressId, Method.GET);

            request.AddHeader("Content-Type", "application/json");

            string token = MixinUtils.GenJwtAuthCode("GET", req + addressId, "", userConfig.ClientId, userConfig.SessionId, priKey);

            var jwtAuth = new RestSharp.Authenticators.JwtAuthenticator(token);
            jwtAuth.Authenticate(client, request);

            var response = client.Execute<Data>(request);

            if (null == response.Data.data)
            {
                var errorinfo = JsonConvert.DeserializeObject<MixinError>(response.Content);
                throw new MixinException(errorinfo);
            }

            var rz = JsonConvert.DeserializeObject<Address>(response.Data.data);

            return rz;

        }

        /// <summary>
        /// Withdrawals the addresses.
        /// </summary>
        /// <returns>The addresses.</returns>
        /// <param name="assetId">Asset identifier.</param>
        public List<Address> WithdrawalAddresses(string assetId)
        {
            CheckAuth();

            string req = "/assets/" + assetId + "/addresses";

            var request = new RestRequest(req, Method.GET);

            request.AddHeader("Content-Type", "application/json");

            string token = MixinUtils.GenJwtAuthCode("GET", req, "", userConfig.ClientId, userConfig.SessionId, priKey);

            var jwtAuth = new RestSharp.Authenticators.JwtAuthenticator(token);
            jwtAuth.Authenticate(client, request);

            var response = client.Execute<Data>(request);

            if (null == response.Data.data)
            {
                var errorinfo = JsonConvert.DeserializeObject<MixinError>(response.Content);
                throw new MixinException(errorinfo);
            }

            var rz = JsonConvert.DeserializeObject<List<Address>>(response.Data.data);

            return rz;

        }

        /// <summary>
        /// Reads the asset.
        /// </summary>
        /// <returns>The asset.</returns>
        /// <param name="asset">Asset.</param>
        public object ReadAsset(string asset)
        {
            return Deposit(asset);
        }

        /// <summary>
        /// Reads the assets.
        /// </summary>
        /// <returns>The assets.</returns>
        public List<Asset> ReadAssets()
        {
            CheckAuth();

            const string req = "/assets";

            var request = new RestRequest(req, Method.GET);

            request.AddHeader("Content-Type", "application/json");

            string token = MixinUtils.GenJwtAuthCode("GET", req, "", userConfig.ClientId, userConfig.SessionId, priKey);

            var jwtAuth = new RestSharp.Authenticators.JwtAuthenticator(token);
            jwtAuth.Authenticate(client, request);

            var response = client.Execute<Data>(request);

            if (null == response.Data.data)
            {
                var errorinfo = JsonConvert.DeserializeObject<MixinError>(response.Content);
                throw new MixinException(errorinfo);
            }

            var rz = JsonConvert.DeserializeObject<List<Asset>>(response.Data.data);

            return rz;
        }


        /// <summary>
        /// Verifies the payment.
        /// </summary>
        /// <returns>The payment.</returns>
        /// <param name="assetId">Asset identifier.</param>
        /// <param name="opponentId">Opponent identifier.</param>
        /// <param name="amount">Amount.</param>
        /// <param name="traceId">Trace identifier.</param>
        public VerifyPaymentRsp VerifyPayment(string assetId, string opponentId, string amount, string traceId)
        {
            CheckAuth();

            const string req = "/payments";

            var request = new RestRequest(req, Method.POST);

            VerifyPaymentReq p = new VerifyPaymentReq
            {
                asset_id = assetId,
                opponent_id = opponentId,
                amount = amount,
                trace_id = traceId
            };

            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(p);

            string token = MixinUtils.GenJwtAuthCode("POST", req, JsonConvert.SerializeObject(p), userConfig.ClientId, userConfig.SessionId, priKey);

            var jwtAuth = new RestSharp.Authenticators.JwtAuthenticator(token);
            jwtAuth.Authenticate(client, request);

            var response = client.Execute<Data>(request);

            if (null == response.Data.data)
            {
                var errorinfo = JsonConvert.DeserializeObject<MixinError>(response.Content);
                throw new MixinException(errorinfo);
            }

            var rz = JsonConvert.DeserializeObject<VerifyPaymentRsp>(response.Data.data);

            return rz;
        }


        /// <summary>
        /// Transfer the specified assetId, opponentId, amount, pin, traceId and memo.
        /// </summary>
        /// <returns>The transfer.</returns>
        /// <param name="assetId">Asset identifier.</param>
        /// <param name="opponentId">Opponent identifier.</param>
        /// <param name="amount">Amount.</param>
        /// <param name="pin">Pin.</param>
        /// <param name="traceId">Trace identifier.</param>
        /// <param name="memo">Memo.</param>
        public Transfer Transfer(string assetId, string opponentId, string amount, string pin, string traceId, string memo)
        {
            CheckAuth();

            const string req = "/transfers";

            var pinBlock = MixinUtils.GenEncrypedPin(pin, userConfig.PinToken, userConfig.SessionId, rsaParameters);

            var request = new RestRequest(req, Method.POST);

            TransferReq p = new TransferReq
            {
                asset_id = assetId,
                opponent_id = opponentId,
                amount = amount,
                trace_id = traceId,
                pin = pinBlock,
                memo = memo
            };

            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(p);

            string token = MixinUtils.GenJwtAuthCode("POST", req, JsonConvert.SerializeObject(p), userConfig.ClientId, userConfig.SessionId, priKey);

            var jwtAuth = new RestSharp.Authenticators.JwtAuthenticator(token);
            jwtAuth.Authenticate(client, request);

            var response = client.Execute<Data>(request);

            if (null == response.Data.data)
            {
                var errorinfo = JsonConvert.DeserializeObject<MixinError>(response.Content);
                throw new MixinException(errorinfo);
            }

            var rz = JsonConvert.DeserializeObject<Transfer>(response.Data.data);

            return rz;
        }

        /// <summary>
        /// Reads the transfer.
        /// </summary>
        /// <returns>The transfer.</returns>
        /// <param name="traceId">Trace identifier.</param>
        public Transfer ReadTransfer(string traceId)
        {
            CheckAuth();

            string req = "/transfers/trace/" + traceId;

            var request = new RestRequest(req, Method.GET);

            request.AddHeader("Content-Type", "application/json");

            string token = MixinUtils.GenJwtAuthCode("GET", req, "", userConfig.ClientId, userConfig.SessionId, priKey);

            var jwtAuth = new RestSharp.Authenticators.JwtAuthenticator(token);
            jwtAuth.Authenticate(client, request);

            var response = client.Execute<Data>(request);

            if (null == response.Data.data)
            {
                var errorinfo = JsonConvert.DeserializeObject<MixinError>(response.Content);
                throw new MixinException(errorinfo);
            }

            var rz = JsonConvert.DeserializeObject<Transfer>(response.Data.data);

            return rz;

        }

        /// <summary>
        /// Tops the assets.
        /// </summary>
        /// <returns>The assets.</returns>
        public List<Asset> TopAssets()
        {
            string req = "network/assets/top";

            var request = new RestRequest(req, Method.GET);

            request.AddHeader("Content-Type", "application/json");

            var response = client.Execute<Data>(request);

            if (null == response.Data.data)
            {
                var errorinfo = JsonConvert.DeserializeObject<MixinError>(response.Content);
                throw new MixinException(errorinfo);
            }

            var rz = JsonConvert.DeserializeObject<List<Asset>>(response.Data.data);

            return rz;
        }


        /// <summary>
        /// Networks the asset.
        /// </summary>
        /// <returns>The asset.</returns>
        /// <param name="assetId">Asset identifier.</param>
        public NetworkAsset NetworkAsset(string assetId)
        {
            CheckAuth();

            string req = "/network/assets/" + assetId;

            var request = new RestRequest(req, Method.GET);

            request.AddHeader("Content-Type", "application/json");

            string token = MixinUtils.GenJwtAuthCode("GET", req, "", userConfig.ClientId, userConfig.SessionId, priKey);

            var jwtAuth = new RestSharp.Authenticators.JwtAuthenticator(token);
            jwtAuth.Authenticate(client, request);

            var response = client.Execute<Data>(request);

            if (null == response.Data.data)
            {
                var errorinfo = JsonConvert.DeserializeObject<MixinError>(response.Content);
                throw new MixinException(errorinfo);
            }

            var rz = JsonConvert.DeserializeObject<NetworkAsset>(response.Data.data);

            return rz;

        }

        /// <summary>
        /// Networks the snapshots.
        /// </summary>
        /// <returns>The snapshots.</returns>
        /// <param name="limit">Limit.</param>
        /// <param name="offset">Offset.</param>
        /// <param name="assetId">Asset identifier.</param>
        /// <param name="order">Order.</param>
        /// <param name="isAuth">If set to <c>true</c> is auth.</param>
        public List<Snapshot> NetworkSnapshots(int limit, string offset, string assetId, string order, bool isAuth)
        {
            string req = "/network/snapshots";

            var request = new RestRequest(req, Method.GET);

            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("limit", limit, ParameterType.QueryString);
            request.AddParameter("offset", offset, ParameterType.QueryString);
            if (!string.IsNullOrEmpty(assetId))
            {
                request.AddParameter("asset", assetId);
            }
            if (!string.IsNullOrEmpty(order))
            {
                request.AddParameter("order", order);
            }

            if (isAuth)
            {
                string token = MixinUtils.GenJwtAuthCode("GET", req, "", userConfig.ClientId, userConfig.SessionId, priKey);

                var jwtAuth = new RestSharp.Authenticators.JwtAuthenticator(token);
                jwtAuth.Authenticate(client, request);
            }

            var response = client.Execute<Data>(request);

            if (null == response.Data.data)
            {
                var errorinfo = JsonConvert.DeserializeObject<MixinError>(response.Content);
                throw new MixinException(errorinfo);
            }

            var rz = JsonConvert.DeserializeObject<List<Snapshot>>(response.Data.data);

            return rz;
        }

        /// <summary>
        /// Networks the snapshot.
        /// </summary>
        /// <returns>The snapshot.</returns>
        /// <param name="snapshotId">Snapshot identifier.</param>
        /// <param name="isAuth">If set to <c>true</c> is auth.</param>
        public Snapshot NetworkSnapshot(string snapshotId, bool isAuth)
        {

            string req = "/network/snapshots/" + snapshotId;

            var request = new RestRequest(req, Method.GET);

            request.AddHeader("Content-Type", "application/json");

            if (isAuth)
            {
                string token = MixinUtils.GenJwtAuthCode("GET", req, "", userConfig.ClientId, userConfig.SessionId, priKey);

                var jwtAuth = new RestSharp.Authenticators.JwtAuthenticator(token);
                jwtAuth.Authenticate(client, request);
            }

            var response = client.Execute<Data>(request);

            if (null == response.Data.data)
            {
                var errorinfo = JsonConvert.DeserializeObject<MixinError>(response.Content);
                throw new MixinException(errorinfo);
            }

            var rz = JsonConvert.DeserializeObject<Snapshot>(response.Data.data);

            return rz;
        }

        /// <summary>
        /// Externals the transactions.
        /// </summary>
        /// <returns>The transactions.</returns>
        /// <param name="assetId">Asset identifier.</param>
        /// <param name="publicKey">Public key.</param>
        /// <param name="accountTag">Account tag.</param>
        /// <param name="accountName">Account name.</param>
        /// <param name="limit">Limit.</param>
        /// <param name="offset">Offset.</param>
        public List<ExternalTransation> ExternalTransactions(string assetId, string publicKey, string accountTag, string accountName, int? limit, string offset)
        {

            string req = "/external/transactions";

            var request = new RestRequest(req, Method.GET);

            request.AddHeader("Content-Type", "application/json");

            if (!string.IsNullOrEmpty(assetId))
            {
                request.AddParameter("asset", assetId);
            }

            if (!string.IsNullOrEmpty(publicKey))
            {
                request.AddParameter("public_key", publicKey);
            }

            if (!string.IsNullOrEmpty(accountTag))
            {
                request.AddParameter("account_tag", accountTag);
            }

            if (!string.IsNullOrEmpty(accountName))
            {
                request.AddParameter("account_name", accountName);
            }

            if (null != limit)
            {
                request.AddParameter("limit", limit, ParameterType.QueryString);
            }

            if (!string.IsNullOrEmpty(offset))
            {
                request.AddParameter("offset", offset, ParameterType.QueryString);
            }

            var response = client.Execute<Data>(request);

            if (null == response.Data.data)
            {
                var errorinfo = JsonConvert.DeserializeObject<MixinError>(response.Content);
                throw new MixinException(errorinfo);
            }

            var rz = JsonConvert.DeserializeObject<List<ExternalTransation>>(response.Data.data);

            return rz;
        }

        /// <summary>
        /// Searchs the assets.
        /// </summary>
        /// <returns>The assets.</returns>
        /// <param name="assetName">Asset name.</param>
        public List<Asset> SearchAssets(string assetName)
        {
            string req = "/network/assets/search/" + assetName;

            var request = new RestRequest(req, Method.GET);

            request.AddHeader("Content-Type", "application/json");

            var response = client.Execute<Data>(request);

            if (null == response.Data.data)
            {
                var errorinfo = JsonConvert.DeserializeObject<MixinError>(response.Content);
                throw new MixinException(errorinfo);
            }

            var rz = JsonConvert.DeserializeObject<List<Asset>>(response.Data.data);

            return rz;

        }


        /// <summary>
        /// Add a app user.
        /// </summary>
        /// <returns>User info</returns>
        /// <param name="fullName">Full name.</param>
        /// <param name="sessionSecret">Session secret.</param>
        public UserInfo APPUser(string fullName, string sessionSecret)
        {
            CheckAuth();

            const string req = "/users";

            var request = new RestRequest(req, Method.POST);

            NewUser p = new NewUser
            {
                full_name = fullName,
                session_secret = sessionSecret
            };

            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(p);

            string token = MixinUtils.GenJwtAuthCode("POST", req, JsonConvert.SerializeObject(p), userConfig.ClientId, userConfig.SessionId, priKey);

            var jwtAuth = new RestSharp.Authenticators.JwtAuthenticator(token);
            jwtAuth.Authenticate(client, request);

            var response = client.Execute<Data>(request);

            if (null == response.Data.data)
            {
                var errorinfo = JsonConvert.DeserializeObject<MixinError>(response.Content);
                throw new MixinException(errorinfo);
            }

            var rz = JsonConvert.DeserializeObject<UserInfo>(response.Data.data);

            return rz;
        }
    }
}
