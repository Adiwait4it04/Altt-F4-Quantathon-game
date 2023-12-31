﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Nethereum.JsonRpc.Client;
using Nethereum.JsonRpc.Client.RpcMessages;

namespace Thirdweb.AccountAbstraction
{
    public class SmartWalletClient : ClientBase
    {
        private SmartWallet _smartWallet;

        public SmartWalletClient(SmartWallet smartWallet)
        {
            this._smartWallet = smartWallet;
        }

        private static readonly Random rng = new Random();
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long GenerateRpcId()
        {
            var date = (long)((DateTime.UtcNow - UnixEpoch).TotalMilliseconds) * (10L * 10L * 10L);
            var extra = (long)Math.Floor(rng.NextDouble() * (10.0 * 10.0 * 10.0));
            return date + extra;
        }

        protected override async Task<RpcResponseMessage> SendAsync(RpcRequestMessage message, string route = null)
        {
            message.Id = GenerateRpcId();
            return await _smartWallet.Request(message);
        }

        protected override Task<RpcResponseMessage[]> SendAsync(RpcRequestMessage[] requests)
        {
            return Task.WhenAll(requests.Select(r => SendAsync(r)));
        }
    }
}
