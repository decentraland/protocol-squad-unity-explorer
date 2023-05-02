// ReSharper disable InconsistentNaming

using Newtonsoft.Json;
using UnityEngine;

namespace RemoteData
{
    public class SceneData
    {
        public ContentData[] content;
        public Metadata metadata;
        public PointerData[] pointers;
        public long timestamp;
        public string type;
        public string version;

        public class ContentData
        {
            public string file;
            public string hash;
        }

        public class PointerData // TODO: fix that 
        {
            public bool stub;
        }

        public class Metadata
        {
            public ContactData contact;
            public DisplayData display;
            public bool ecs7;
            public string main;

            public string name;
            public string owner;
            public string[] requiredPermissions;
            public string runtimeVersion;
            public SceneData scene;
            public SpawnPointsData[] spawnPoints;
            public string[] tags;

            public class DisplayData
            {
                public string favicon;
                public string title;
            }

            public class ContactData
            {
                public string email;
                public string name;
            }

            public class SceneData
            {
                [JsonProperty("base")] public Vector2Int Base;

                public Vector2Int[] parcels;
            }

            public class SpawnPointsData
            {
                // position // TODO - implement this
                public Vector3 cameraTarget;

                [JsonProperty("default")] public bool isDefault;

                public string name;
            }
        }
    }
}