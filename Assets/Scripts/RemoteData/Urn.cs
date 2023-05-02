namespace RemoteData
{
    public class Urn
    {
        public readonly string BaseUrl;
        public readonly string Hash;

        internal Urn(string hash, string baseUrl)
        {
            Hash = hash;
            BaseUrl = baseUrl;
        }

        public string URL => $"{BaseUrl}{Hash}";
    }
}