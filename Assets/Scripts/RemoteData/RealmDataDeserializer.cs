using Newtonsoft.Json;

namespace RemoteData
{
    public static class RealmDataDeserializer
    {
        public static RealmData ToRealmData(this string jsonString)
        {
            return JsonConvert.DeserializeObject<RealmData>(jsonString);
        }
    }
}