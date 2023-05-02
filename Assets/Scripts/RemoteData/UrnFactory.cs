using System;
using UnityEngine.Assertions;

namespace RemoteData
{
    public class UrnFactory
    {
        private const string URN_PREFIX = "urn:decentraland:entity:";
        
        private readonly string _baseUrl;

        public UrnFactory(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public Urn Create(string assetHash)
        {
            return new Urn(assetHash, _baseUrl);
        }
        
        // take input parameter as "urn:decentraland:entity:bafkreidvjhbobdwnnhwyaqbxskwyaxwc5cfuilzqa3gho6lqu7abcrwsti?=&baseUrl=https://sdk-team-cdn.decentraland.org/ipfs/",
        public static Urn FormatString(string str)
        {
            Assert.IsTrue(str.StartsWith(URN_PREFIX), $"Unsupported urn string {str}");
            
            var hash = str.Substring(URN_PREFIX.Length, str.IndexOf("?", StringComparison.Ordinal) - URN_PREFIX.Length);
            var baseUrl = str.Split("baseUrl=")[1];
            return new Urn(hash, baseUrl);
        }
    }
}