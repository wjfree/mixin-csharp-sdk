using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MixinSdk.Bean;
using Newtonsoft.Json;
using RestSharp;

namespace MixinSdk
{
    public partial class MixinApi
    {
        /// <summary>
        /// Create or update user's pin code.
        /// </summary>
        /// <returns>User Info</returns>
        /// <param name="oldPin">Old pin or null</param>
        /// <param name="newPin">New pin.</param>
        public UserInfo CreatePIN(string oldPin, string newPin)
        {
            return CreatePINAsync(oldPin, newPin).Result;
        }

        /// <summary>
        /// Creates the PIN Async.
        /// </summary>
        /// <returns>The PINA sync.</returns>
        /// <param name="oldPin">Old pin.</param>
        /// <param name="newPin">New pin.</param>
        public async Task<UserInfo> CreatePINAsync(string oldPin, string newPin)
        {
            const string req = "/pin/update";

            var oldPinBlock = "";
            if (!string.IsNullOrEmpty(oldPin))
            {
                oldPinBlock = GenEncrypedPin(oldPin);
            }
            var newPinBlock = GenEncrypedPin(newPin);

            var p = new CreatePinReq
            {
                old_pin = oldPinBlock,
                pin = newPinBlock
            };

            var rz = await doPostRequestAsync(req, p, true);

            return JsonConvert.DeserializeObject<UserInfo>(rz);
        }

        /// <summary>
        /// Verifie pin.
        /// </summary>
        /// <returns>User Info</returns>
        /// <param name="pin">Pin.</param>
        public UserInfo VerifyPIN(string pin)
        {
            return VerifyPINAsync(pin).Result;
        }

        /// <summary>
        /// Verifies the PIN Async.
        /// </summary>
        /// <returns>The PINA sync.</returns>
        /// <param name="pin">Pin.</param>
        public async Task<UserInfo> VerifyPINAsync(string pin)
        {
            const string req = "/pin/verify";

            var pinBlock = GenEncrypedPin(pin);

            VerifyPinReq p = new VerifyPinReq
            {
                pin = pinBlock
            };

            var rz = await doPostRequestAsync(req, p, true);

            return JsonConvert.DeserializeObject<UserInfo>(rz);
        }

        /// <summary>
        /// Deposit the specified assetID.
        /// </summary>
        /// <returns>The deposit.</returns>
        /// <param name="assetID">Asset identifier.</param>
        public Asset Deposit(string assetID, string token = null)
        {
            return DepositAsync(assetID, token).Result;
        }

        /// <summary>
        /// Deposits the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="assetID">Asset identifier.</param>
        /// <param name="token">Token.</param>
        public async Task<Asset> DepositAsync(string assetID, string token = null)
        {
            string req = "/assets/" + assetID;

            var rz = await doGetRequestAsync(req, true, token);

            return JsonConvert.DeserializeObject<Asset>(rz);
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
            return WithdrawalAsync(addressId, amount, pin, traceId, memo).Result;
        }

        /// <summary>
        /// Withdrawals the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="addressId">Address identifier.</param>
        /// <param name="amount">Amount.</param>
        /// <param name="pin">Pin.</param>
        /// <param name="traceId">Trace identifier.</param>
        /// <param name="memo">Memo.</param>
        public async Task<WithDrawalInfo> WithdrawalAsync(string addressId, string amount, string pin, string traceId, string memo)
        {
            const string req = "/withdrawals";

            var pinBlock = GenEncrypedPin(pin);

            WithDrawalReq p = new WithDrawalReq
            {
                address_id = addressId,
                amount = amount,
                pin = pinBlock,
                trace_id = traceId,
                memo = memo
            };

            var rz = await doPostRequestAsync(req, p, true);

            return JsonConvert.DeserializeObject<WithDrawalInfo>(rz);
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
            return CreateAddressAsync(assetId, publicKey, label, accountName, accountTag, pin).Result;
        }

        /// <summary>
        /// Creates the address async.
        /// </summary>
        /// <returns>The address async.</returns>
        /// <param name="assetId">Asset identifier.</param>
        /// <param name="publicKey">Public key.</param>
        /// <param name="label">Label.</param>
        /// <param name="accountName">Account name.</param>
        /// <param name="accountTag">Account tag.</param>
        /// <param name="pin">Pin.</param>
        public async Task<Address> CreateAddressAsync(string assetId, string publicKey, string label, string accountName, string accountTag, string pin)
        {
            const string req = "/addresses";

            var pinBlock = GenEncrypedPin(pin);

            CreateAddressReq p = new CreateAddressReq
            {
                asset_id = assetId,
                public_key = publicKey,
                label = label,
                account_name = accountName,
                account_tag = accountTag,
                pin = pinBlock
            };

            var rz = await doPostRequestAsync(req, p, true);

            return JsonConvert.DeserializeObject<Address>(rz);
        }

        /// <summary>
        /// Deletes the address.
        /// </summary>
        /// <returns><c>true</c>, if address was deleted, <c>false</c> otherwise.</returns>
        /// <param name="pin">Pin.</param>
        /// <param name="addressId">Address identifier.</param>
        public bool DeleteAddress(string pin, string addressId)
        {
            return DeleteAddressAsync(pin, addressId).Result;
        }

        /// <summary>
        /// Deletes the address async.
        /// </summary>
        /// <returns>The address async.</returns>
        /// <param name="pin">Pin.</param>
        /// <param name="addressId">Address identifier.</param>
        public async Task<bool> DeleteAddressAsync(string pin, string addressId)
        {
            string req = "/addresses/" + addressId + "/delete";

            var pinBlock = GenEncrypedPin(pin);

            VerifyPinReq p = new VerifyPinReq
            {
                pin = pinBlock
            };

            var rz = await doPostRequestAsync(req, p, true);
            return true;
        }

        /// <summary>
        /// Reads the address.
        /// </summary>
        /// <returns>The address.</returns>
        /// <param name="addressId">Address identifier.</param>
        public Address ReadAddress(string addressId)
        {
            return ReadAddressAsync(addressId).Result;
        }

        /// <summary>
        /// Reads the address async.
        /// </summary>
        /// <returns>The address async.</returns>
        /// <param name="addressId">Address identifier.</param>
        public async Task<Address> ReadAddressAsync(string addressId)
        {
            string req = "/addresses/" + addressId;
            var rz = await doGetRequestAsync(req, true);
            return JsonConvert.DeserializeObject<Address>(rz);
        }

        /// <summary>
        /// Withdrawals the addresses.
        /// </summary>
        /// <returns>The addresses.</returns>
        /// <param name="assetId">Asset identifier.</param>
        public List<Address> WithdrawalAddresses(string assetId)
        {
            return WithdrawalAddressesAsync(assetId).Result;
        }

        /// <summary>
        /// Withdrawals the addresses async.
        /// </summary>
        /// <returns>The addresses async.</returns>
        /// <param name="assetId">Asset identifier.</param>
        public async Task<List<Address>> WithdrawalAddressesAsync(string assetId)
        {
            string req = "/assets/" + assetId + "/addresses";
            var rz = await doGetRequestAsync(req, true);
            return JsonConvert.DeserializeObject<List<Address>>(rz);
        }

        /// <summary>
        /// Reads the asset.
        /// </summary>
        /// <returns>The asset.</returns>
        /// <param name="asset">Asset.</param>
        public Asset ReadAsset(string asset, string token = null)
        {
            return Deposit(asset, token);
        }

        /// <summary>
        /// Reads the asset async.
        /// </summary>
        /// <returns>The asset async.</returns>
        /// <param name="asset">Asset.</param>
        /// <param name="token">Token.</param>
        public async Task<Asset> ReadAssetAsync(string asset, string token = null)
        {
            return await DepositAsync(asset, token);
        }

        /// <summary>
        /// Reads the assets.
        /// </summary>
        /// <returns>The assets.</returns>
        public List<Asset> ReadAssets(string token = null)
        {
            return ReadAssetsAsync(token).Result;
        }

        /// <summary>
        /// Reads the assets async.
        /// </summary>
        /// <returns>The assets async.</returns>
        /// <param name="token">Token.</param>
        public async Task<List<Asset>> ReadAssetsAsync(string token = null)
        {
            const string req = "/assets";
            var rz = await doGetRequestAsync(req, true, token);
            return JsonConvert.DeserializeObject<List<Asset>>(rz);
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
            return VerifyPaymentAsync(assetId, opponentId, amount, traceId).Result;
        }

        /// <summary>
        /// Verifies the payment async.
        /// </summary>
        /// <returns>The payment async.</returns>
        /// <param name="assetId">Asset identifier.</param>
        /// <param name="opponentId">Opponent identifier.</param>
        /// <param name="amount">Amount.</param>
        /// <param name="traceId">Trace identifier.</param>
        public async Task<VerifyPaymentRsp> VerifyPaymentAsync(string assetId, string opponentId, string amount, string traceId)
        {
            const string req = "/payments";

            VerifyPaymentReq p = new VerifyPaymentReq
            {
                asset_id = assetId,
                opponent_id = opponentId,
                amount = amount,
                trace_id = traceId
            };

            var rz = await doPostRequestAsync(req, p, true);

            return JsonConvert.DeserializeObject<VerifyPaymentRsp>(rz);
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
            return TransferAsync(assetId, opponentId, amount, pin, traceId, memo).Result;
        }

        /// <summary>
        /// Transfers the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="assetId">Asset identifier.</param>
        /// <param name="opponentId">Opponent identifier.</param>
        /// <param name="amount">Amount.</param>
        /// <param name="pin">Pin.</param>
        /// <param name="traceId">Trace identifier.</param>
        /// <param name="memo">Memo.</param>
        public async Task<Transfer> TransferAsync(string assetId, string opponentId, string amount, string pin, string traceId, string memo)
        {
            const string req = "/transfers";

            var pinBlock = GenEncrypedPin(pin);

            TransferReq p = new TransferReq
            {
                asset_id = assetId,
                opponent_id = opponentId,
                amount = amount,
                trace_id = traceId,
                pin = pinBlock,
                memo = memo
            };

            var rz = await doPostRequestAsync(req, p, true);

            return JsonConvert.DeserializeObject<Transfer>(rz);
        }

        /// <summary>
        /// Reads the transfer.
        /// </summary>
        /// <returns>The transfer.</returns>
        /// <param name="traceId">Trace identifier.</param>
        public Transfer ReadTransfer(string traceId)
        {
            return ReadTransferAsync(traceId).Result;
        }

        /// <summary>
        /// Reads the transfer async.
        /// </summary>
        /// <returns>The transfer async.</returns>
        /// <param name="traceId">Trace identifier.</param>
        public async Task<Transfer> ReadTransferAsync(string traceId)
        {
            string req = "/transfers/trace/" + traceId;

            var rz = await doGetRequestAsync(req, true);

            return JsonConvert.DeserializeObject<Transfer>(rz);
        }

        /// <summary>
        /// Tops the assets.
        /// </summary>
        /// <returns>The assets.</returns>
        public List<Asset> TopAssets()
        {
            return TopAssetsAsync().Result;
        }

        /// <summary>
        /// Tops the assets async.
        /// </summary>
        /// <returns>The assets async.</returns>
        public async Task<List<Asset>> TopAssetsAsync()
        {
            string req = "network/assets/top";

            var rz = await doGetRequestAsync(req, false);

            return JsonConvert.DeserializeObject<List<Asset>>(rz);
        }


        /// <summary>
        /// Network asset.
        /// </summary>
        /// <returns>The asset.</returns>
        /// <param name="assetId">Asset identifier.</param>
        public NetworkAsset NetworkAsset(string assetId)
        {
            return NetworkAssetAsync(assetId).Result;
        }

        /// <summary>
        /// Networks asset async.
        /// </summary>
        /// <returns>The asset async.</returns>
        /// <param name="assetId">Asset identifier.</param>
        public async Task<NetworkAsset> NetworkAssetAsync(string assetId)
        {
            string req = "/network/assets/" + assetId;

            var rz = await doGetRequestAsync(req, true);

            return JsonConvert.DeserializeObject<NetworkAsset>(rz);
        }

        /// <summary>
        /// Network snapshots.
        /// </summary>
        /// <returns>The snapshots.</returns>
        /// <param name="limit">Limit.</param>
        /// <param name="offset">Offset.</param>
        /// <param name="assetId">Asset identifier.</param>
        /// <param name="order">Order.</param>
        /// <param name="isAuth">If set to <c>true</c> is auth.</param>
        public List<Snapshot> NetworkSnapshots(int limit, string offset, string assetId, string order, bool isAuth)
        {
            return NetworkSnapshotsAsync(limit, offset, assetId, order, isAuth).Result;
        }

        /// <summary>
        /// Network snapshots async.
        /// </summary>
        /// <returns>The snapshots async.</returns>
        /// <param name="limit">Limit.</param>
        /// <param name="offset">Offset.</param>
        /// <param name="assetId">Asset identifier.</param>
        /// <param name="order">Order.</param>
        /// <param name="isAuth">If set to <c>true</c> is auth.</param>
        public async Task<List<Snapshot>> NetworkSnapshotsAsync(int limit, string offset, string assetId, string order, bool isAuth)
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

            var cts = new CancellationTokenSource(ReadTimeout);
            var response = await client.ExecuteTaskAsync<Data>(request, cts.Token);

            if (null == response.Data.data)
            {
                var errorinfo = JsonConvert.DeserializeObject<MixinError>(response.Content);
                throw new MixinException(errorinfo);
            }

            var rz = JsonConvert.DeserializeObject<List<Snapshot>>(response.Data.data);

            return rz;
        }

        /// <summary>
        /// Network snapshot.
        /// </summary>
        /// <returns>The snapshot.</returns>
        /// <param name="snapshotId">Snapshot identifier.</param>
        /// <param name="isAuth">If set to <c>true</c> is auth.</param>
        public Snapshot NetworkSnapshot(string snapshotId, bool isAuth)
        {
            return NetworkSnapshotAsync(snapshotId, isAuth).Result;
        }

        /// <summary>
        /// Network snapshot async.
        /// </summary>
        /// <returns>The snapshot async.</returns>
        /// <param name="snapshotId">Snapshot identifier.</param>
        /// <param name="isAuth">If set to <c>true</c> is auth.</param>
        public async Task<Snapshot> NetworkSnapshotAsync(string snapshotId, bool isAuth)
        {
            string req = "/network/snapshots/" + snapshotId;

            var rz = await doGetRequestAsync(req, isAuth);
            return JsonConvert.DeserializeObject<Snapshot>(rz);
        }

        /// <summary>
        /// External transactions.
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
            return ExternalTransactionsAsync(assetId, publicKey, accountTag, accountName, limit, offset).Result;
        }

        /// <summary>
        /// External transactions async.
        /// </summary>
        /// <returns>The transactions async.</returns>
        /// <param name="assetId">Asset identifier.</param>
        /// <param name="publicKey">Public key.</param>
        /// <param name="accountTag">Account tag.</param>
        /// <param name="accountName">Account name.</param>
        /// <param name="limit">Limit.</param>
        /// <param name="offset">Offset.</param>
        public async Task<List<ExternalTransation>> ExternalTransactionsAsync(string assetId, string publicKey, string accountTag, string accountName, int? limit, string offset)
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

            var cts = new CancellationTokenSource(ReadTimeout);
            var response = await client.ExecuteTaskAsync<Data>(request, cts.Token);

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
            return SearchAssetsAsync(assetName).Result;
        }

        /// <summary>
        /// Searchs the assets async.
        /// </summary>
        /// <returns>The assets async.</returns>
        /// <param name="assetName">Asset name.</param>
        public async Task<List<Asset>> SearchAssetsAsync(string assetName)
        {
            string req = "/network/assets/search/" + assetName;

            var rz = await doGetRequestAsync(req, false);

            return JsonConvert.DeserializeObject<List<Asset>>(rz);
        }

        /// <summary>
        /// Add a app user.
        /// </summary>
        /// <returns>User info</returns>
        /// <param name="fullName">Full name.</param>
        /// <param name="sessionSecret">Session secret.</param>
        public UserInfo APPUser(string fullName, string sessionSecret)
        {
            return APPUserAsync(fullName, sessionSecret).Result;
        }

        public async Task<UserInfo> APPUserAsync(string fullName, string sessionSecret)
        {
            const string req = "/users";

            NewUser p = new NewUser
            {
                full_name = fullName,
                session_secret = sessionSecret
            };

            var rz = await doPostRequestAsync(req, p, true);
            return JsonConvert.DeserializeObject<UserInfo>(rz);
        }
    }
}
