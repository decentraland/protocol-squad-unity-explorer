
// ReSharper disable InconsistentNaming

namespace RemoteData
{
    public class RealmData
    {
        public bool acceptingUsers;
        public UrlWithStatus bff;
        public Comms comms;
        public Configurations configurations;
        public UrlWithStatus content;
        public UrlWithStatus lambdas;
        public bool healthy;
        
        public class UrlWithStatus
        {
            public bool healthy;
            public string publicUrl;
        }

        public class Comms
        {
            public bool healthy;
            public string protocol;
            public string fixedAdapter;
        }
        
        public class Configurations
        {
            public int networkId;
            public string[] globalScenesUrn;
            public string[] scenesUrn;
            public string realmName;
        }
    }
}
