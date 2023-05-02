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
        public bool healthy;
        public UrlWithStatus lambdas;

        public class UrlWithStatus
        {
            public bool healthy;
            public string publicUrl;
        }

        public class Comms
        {
            public string fixedAdapter;
            public bool healthy;
            public string protocol;
        }

        public class Configurations
        {
            public string[] globalScenesUrn;
            public int networkId;
            public string realmName;
            public string[] scenesUrn;
        }
    }
}