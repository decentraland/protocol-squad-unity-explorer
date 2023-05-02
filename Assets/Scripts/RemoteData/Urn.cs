
namespace RemoteData
{
    public class Urn
    {
        public readonly string Hash;
        public readonly string BaseUrl;
        public string URL => $"{BaseUrl}{Hash}";
        
        internal Urn(string hash, string baseUrl)
        {
            Hash = hash;
            BaseUrl = baseUrl;
        }
    }
}